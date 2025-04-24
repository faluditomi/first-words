using System;
using UnityEngine;

public abstract class Spell<TArgs> : ScriptableObject where TArgs : SpellArgs
{
    
    public SpellWords spellWord;
    public float cooldownDuration;
    public event Action<TArgs> cast;

    public void Cast()
    {
        TArgs args = CreateArgs();
        Debug.Log($"{spellWord} triggered" + args != null ? $" with args: {args}." : ".");
        cast?.Invoke(args);
    }

    //NOTE
    // if needed, override this in child classes to provide event properties (see ExampleComplexSpell)
    protected abstract TArgs CreateArgs();

}

public class SpellArgs
{

    // extended by the specific spell's event properties class

}
