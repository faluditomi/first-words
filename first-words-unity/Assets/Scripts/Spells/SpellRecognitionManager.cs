using System.Collections.Generic;
using UnityEngine;

public class SpellRecognitionManager : MonoBehaviour
{

    public static SpellRecognitionManager Instance { get; private set; }
    
    [SerializeField] private List<SpellWords> activeSpells;

    private List<SpellWords> spellsInCurrentSegment = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SessionSpellCache.LoadSessionSpells(activeSpells);
    }
    
    private void OnDisable()
    {
        SessionSpellCache.UnloadAll();
    }

    public void ScanSegment(string segment)
    {
        segment = RemoveRegisteredSpells(segment);

        foreach(SpellWords spellWord in activeSpells)
        {
            //REVIEW
            // will it work like this, or do we have to somehow wait until the recursion executes fully?
            // maybe use multithreading here?
            CheckForSpell(segment, spellWord);
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

    private void CheckForSpell(string segment, SpellWords spellWord)
    {
        if(!ContainsSpellStringUtil(segment, spellWord))
        {
            return;
        }

        spellsInCurrentSegment.Add(spellWord);
        //TODO
        // check if the spellWord is of type "i need context from speech" and pass that context along somehow
        SessionSpellCache.CastSpell(spellWord);
        segment = RemoveSpellStringUtil(segment, spellWord);
        CheckForSpell(segment, spellWord);
    }

    public void ResetSegmentation()
    {
        //TODO
        // maybe make the checks into a coroutine, so we can stop them here
        // (so it doesn't try to match spells with an empty spellsInCurrentSegment)
        // (or maybe instead of having a global variable, pass the spell list around as a parameter)
        spellsInCurrentSegment = new List<SpellWords>();
    }

    //NOTE
    // these util methods could go in a separate SpellWordUtil class if they are ever to be used in other scripts
    private bool ContainsSpellStringUtil(string context, SpellWords spellToCheck)
    {
        return context.ToLower().Contains(SpellEnumToStringUtil(spellToCheck));
    }

    private string RemoveSpellStringUtil(string context, SpellWords spellToRemove)
    {
        context = context.ToLower();
        string spellWordToRemove = SpellEnumToStringUtil(spellToRemove);
        int index = context.IndexOf(spellWordToRemove);
        context = index != -1 ? context.Remove(index, spellWordToRemove.Length).Trim() : context;

        return context;
    }

    private string SpellEnumToStringUtil(SpellWords spellWord)
    {
        return spellWord.ToString().ToLower().Replace("_", " ");
    }

}
