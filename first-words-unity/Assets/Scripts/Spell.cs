using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/BasicSpell")]
public class Spell : ScriptableObject
{
    
    public SpellWords spellWord;
    public float cooldownDuration;
    public event Action<SpellEventArgs> cast;

    public void Cast()
    {
        SpellEventArgs args = CreateArgs();
        Debug.Log($"{spellWord} triggered" + args != null ? $" with args: {args}." : ".");
        cast?.Invoke(args);
    }

    //NOTE
    // if needed, override this in child classes to provide event args (see ExampleComplexSpell)
    protected virtual SpellEventArgs CreateArgs()
    {
        return null;
    }

}
