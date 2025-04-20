using System.Collections.Generic;
using UnityEngine;

public class SpellRecognitionManager : MonoBehaviour
{

    public static SpellRecognitionManager Instance { get; private set; }
    
    [SerializeField] private List<SpellWords> activeSpells;

    private List<SpellWords> spellsInCurrentSegment;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SessionSpellCache.LoadSessionSpells(activeSpells);
    }

    private void OnDisable()
    {
        SessionSpellCache.UnloadAll();
    }

    public void ScanSegment(string segment)
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

        foreach(SpellWords spellWord in activeSpells)
        {
            //REVIEW
            //will it work like this, or do we have to somehow wait until the recursion executes fully?
            //maybe use multithreading here?
            CheckForSpell(segment, spellWord);
        }
    }

    private void CheckForSpell(string segment, SpellWords spellWord)
    {
        if(!ContainsSpellStringUtil(segment, spellWord))
        {
            return;
        }

        spellsInCurrentSegment.Add(spellWord);
        SessionSpellCache.CastSpell(spellWord);
        segment = RemoveSpellStringUtil(segment, spellWord);
        CheckForSpell(segment, spellWord);
    }

    public void ResetSegmentation()
    {
        //TODO
        //maybe make the checks into a coroutine, so we can stop them here
        //(so it doesn't try to match spells with an empty spellsInCurrentSegment)
        spellsInCurrentSegment = new List<SpellWords>();
    }

    private bool ContainsSpellStringUtil(string context, SpellWords spellToCheck)
    {
        return context.ToLower().Contains(spellToCheck.ToString().ToLower());
    }

    private string RemoveSpellStringUtil(string context, SpellWords spellToRemove)
    {
        context = context.ToLower();
        string spellWordToRemove = spellToRemove.ToString().ToLower();
        int index = context.IndexOf(spellWordToRemove);
        context = index != -1 ? context.Remove(index, spellWordToRemove.Length).Trim() : context;

        return context;
    }

}
