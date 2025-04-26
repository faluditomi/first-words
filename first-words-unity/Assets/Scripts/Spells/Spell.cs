using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Spell")]
public class Spell : ScriptableObject
{
    
    public SpellWords spellWord;
    public float cooldownDuration;
    public event Action<SpellArgs> cast;

    public void Cast()
    {
        SpellArgs args = CreateArgs();
        Debug.Log($"{spellWord} triggered.");
        cast?.Invoke(args);
    }

    public T GetMyArgs<T>(SpellArgs args) where T : SpellArgs
    {
        if(args is T castedArgs)
        {
            return castedArgs;
        }
        else
        {
            Debug.LogError($"{spellWord} was cast with the wrong args type. Expected {typeof(T)}, but got {args.GetType()}.");
            return null;
        }
    }

    //NOTE
    // if needed, override this in child classes to provide event properties (see ExampleComplexSpell)
    // it makes sure that the newly created args also hold the base properties
    protected virtual SpellArgs CreateArgs()
    {
        return new SpellArgs
        {
            spellWord = this.spellWord,
            cooldownDuration = this.cooldownDuration
        };
    }

    //NOTE
    // if CreateArgs is overridden, this method has to be called inside the overwriting method (see ExampleComplexSpell)
    protected SpellArgs CopyTo(SpellArgs target)
    {
        target.spellWord = this.spellWord;
        target.cooldownDuration = this.cooldownDuration;
        return target;
    }

}

public class SpellArgs
{

    public SpellWords spellWord;
    public float cooldownDuration;

}
