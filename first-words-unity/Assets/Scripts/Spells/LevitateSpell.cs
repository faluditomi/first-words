using Unity.Entities;
using UnityEngine;

[CreateAssetMenu(fileName = "LevitateSpell", menuName = "Spells/LevitateSpell")]
public class LevitateSpell : Spell
{
    
    [Min(0f), Tooltip("If the distance between the player and a levitateable object is less then this, it can be interacted with.")]
    public float range;
    [Min(0f), Tooltip("The speed at which the objects fly from their spot to the gathering point in front of the player.")]
    public float gatherSpeed;
    [Min(0f), Tooltip("The intensity with which the levitating objects are moving around while at the gathering point.")]
    public float jitterIntensity;

    //TODO: document this also (kinda not needed, only when the spell has an IBufferElementData)
    static LevitateSpell()
    {
        SpellSerializationRegistry.Register(SpellWords.Levitate, spell =>
        {
            LevitateSpell levitateSpell = (LevitateSpell) spell;
            LevitateBufferPayload bufferElement = new LevitateBufferPayload
            {
                range = levitateSpell.range,
                gatherSpeed = levitateSpell.gatherSpeed,
                jitterIntensity = levitateSpell.jitterIntensity
            };

            return SpellSerializationRegistry.SerializeStruct(bufferElement);
        });
    }

    protected override SpellArgs CreateArgs()
    {
        LevitateArgs myArgs = new LevitateArgs
        {
            range = this.range,
            gatherSpeed = this.gatherSpeed,
            jitterIntensity = this.jitterIntensity
        };

        return CopyTo(myArgs);
    }

}

//REVIEW: this might be useless if the levitateables are using only a system
public class LevitateArgs : SpellArgs
{

    public float range;
    public float gatherSpeed;
    public float jitterIntensity;
    //TODO: add highlight particles here?

}

public struct LevitateBufferPayload : IBufferElementData
{
    
    public float range;
    public float gatherSpeed;
    public float jitterIntensity;

}
