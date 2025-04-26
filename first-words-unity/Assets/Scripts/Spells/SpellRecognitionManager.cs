using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SpellRecognitionManager : MonoBehaviour
{

    public static SpellRecognitionManager _instance { get; private set; }
    
    [SerializeField] private List<SpellWords> activeSpells;
    private ConcurrentBag<SpellWords> spellsInCurrentSegment = new();
    private CancellationTokenSource cancellationTokenSource;
    private Dictionary<SpellWords, string> spellWordCache;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        SessionSpellCache.LoadSessionSpells(activeSpells);
        spellWordCache = activeSpells.ToDictionary(spell => spell, spell => SpellEnumToStringUtil(spell));
    }
    
    private void OnDisable()
    {
        SessionSpellCache.UnloadAll();
    }

    public void ScanSegment(string segment)
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        segment = RemoveRegisteredSpells(segment);

        try
        {
            Parallel.ForEach(activeSpells, new ParallelOptions { CancellationToken = token }, spellWord =>
            {
                CheckForSpell(segment, spellWord, token);
            });
        }
        catch(OperationCanceledException)
        {
            Debug.LogError("ScanSegment operation was canceled before segment got processed.");
        }
    }

    //TODO
    // check if the spellWord is of type "i need context from speech" and pass that context along somehow
    private void CheckForSpell(string segment, SpellWords spellWord, CancellationToken token)
    {
        while(!token.IsCancellationRequested && ContainsSpellStringUtil(segment, spellWord))
        {
            spellsInCurrentSegment.Add(spellWord);
            SessionSpellCache.CastSpell(spellWord);
            // segment = RemoveSpellStringUtil(segment, spellWord);
            segment = RemoveSpellAndBeforeUtil(segment, spellWord);
        }

        if(token.IsCancellationRequested)
        {
            Debug.LogError($"CheckForSpell was canceled for segment: {segment}");
        }
    }

    private string RemoveRegisteredSpells(string segment)
    {
        if(spellsInCurrentSegment.Count > 0)
        {
            foreach(SpellWords spellWord in spellsInCurrentSegment)
            {
                if(ContainsSpellStringUtil(segment, spellWord))
                {
                    segment = RemoveSpellStringUtil(segment, spellWord);
                }
                else
                {
                    Debug.LogWarning($"Potential false positive detected: {spellWord}");
                }
            }
        }

        return segment;
    }

    public void ResetSegmentation()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel(); // Cancel all running tasks
            Debug.Log($"ResetSegmentation: Canceled running tasks.");
        }

        //TODO
        // maybe make the checks into a coroutine, so we can stop them here
        // (so it doesn't try to match spells with an empty spellsInCurrentSegment)
        // (or maybe instead of having a global variable, pass the spell list around as a parameter)
        spellsInCurrentSegment = new ConcurrentBag<SpellWords>();
    }

    //NOTE
    // these util methods could go in a separate SpellWordUtil class if they are ever to be used in other scripts
    private bool ContainsSpellStringUtil(string context, SpellWords spellToCheck)
    {
        return context.ToLower().Contains(spellWordCache[spellToCheck]);
    }

    private string RemoveSpellStringUtil(string context, SpellWords spellToRemove)
    {
        string spellWordToRemove = spellWordCache[spellToRemove];
        int index = context.IndexOf(spellWordToRemove, StringComparison.OrdinalIgnoreCase);
        return index != -1 ? context.Remove(index, spellWordToRemove.Length).Trim() : context;
    }

    private string RemoveSpellAndBeforeUtil(string context, SpellWords spellToRemove)
    {
        string spellWordToRemove = spellWordCache[spellToRemove];
        int index = context.IndexOf(spellWordToRemove, StringComparison.OrdinalIgnoreCase);
        return index != -1 ? context.Substring(index + spellWordToRemove.Length).Trim() : context;
    }

    private string SpellEnumToStringUtil(SpellWords spellWord)
    {
        return spellWord.ToString().ToLower().Replace("_", " ");
    }

}
