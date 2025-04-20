using System.Collections.Generic;
using UnityEngine;

public class SpellRecognitionManager : MonoBehaviour
{
    
    [SerializeField] private List<SpellWords> activeSpells;

    private void Awake()
    {
        SessionSpellCache.LoadSessionSpells(activeSpells);
    }

    void OnDisable()
    {
        SessionSpellCache.UnloadAll();
    }

    //TODO: method that receives a string and looks for spell words in it, and if found any, triggers their Spell's event
    // match text with activeSpells' strings and then find the corresponding spell using the below method
    
    // var spell = SessionSpellCache.GetSpell(spellWord);
    // spell?.Cast();

}
