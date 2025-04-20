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

    //TODO
    //method that receives a string and looks for spell words in it, and if found any, triggers their Spell's event
    // match text with activeSpells' strings and then find the corresponding spell using the below method
    // var spell = SessionSpellCache.GetSpell(spellWord);
    // spell?.Cast();   
    public void ScanSegment(string segment)
    {
        //TODO
        //remove all the spellwords that are currently in spellsInCurrentSegment
        //obviously, first check whether the word really is there (if not, there probably was a false positive, so also log it, maybe make a counter)
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
            //TODO
            //will it work like this, or do we have to somehow wait until the recursion executes fully?
            CheckForSpell(segment, spellWord);
        }
    }

    private void CheckForSpell(string segment, SpellWords spellWord)
    {
        if(ContainsSpellStringUtil(segment, spellWord))
        {
            //TODO
            //remove the string from segment
            //add spell to spellsInCurrentSegment
            //cast spell
            //call this method again
        }
        else
        {
            //TODO
            //return
        }
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

        //TODO: can this if be removed, since the string deffo contains the spell at this point?
        if(index != -1)
        {
            context = context.Remove(index, spellWordToRemove.Length).Trim();
        }

        return context;
    }

}
