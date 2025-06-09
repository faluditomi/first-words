using Unity.Entities;
using UnityEngine;

[CreateAssetMenu(fileName = "LevitateSpell", menuName = "Spells/LevitateSpell")]
public class LevitateSpell : Spell
{
    
    [Min(0f), Tooltip("If the distance between the player and a levitateable object is less then this, it can be interacted with.")]
    public float range;

    //TODO: document this in Spell (kinda not needed, only when the spell has an IBufferElementData)
    static LevitateSpell()
    {
        SpellSerializationRegistry.Register(SpellWords.Levitate, spell =>
        {
            LevitateSpell levitateSpell = (LevitateSpell) spell;
            LevitateBufferPayload bufferElement = new LevitateBufferPayload
            {
                range = levitateSpell.range
            };

            return SpellSerializationRegistry.SerializeStruct(bufferElement);
        });
    }

    protected override SpellArgs CreateArgs()
    {
        LevitateArgs myArgs = new LevitateArgs{};

        return CopyTo(myArgs);
    }

}

public class LevitateArgs : SpellArgs
{

    //TODO: add highlight particles here? ( spell visuals, particle effects )

}

public struct LevitateBufferPayload : IBufferElementData
{
    
    public float range;

}
