using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Spell")]
public class Spell : ScriptableObject
{
    
    public SpellWords spellWord;
    public float cooldownDuration;

    public event Action cast;

    public void Cast()
    {
        Debug.Log($"Spell {spellWord} triggered.");
        cast?.Invoke();
    }

}
