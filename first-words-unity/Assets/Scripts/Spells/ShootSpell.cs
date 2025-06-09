using Unity.Entities;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootSpell", menuName = "Spells/ShootSpell")]
public class ShootSpell : Spell
{
    
    [Min(0f), Tooltip("The force with which the objects are launched.")]
    public float shootForce;

    //TODO: document this in Spell (kinda not needed, only when the spell has an IBufferElementData)
    static ShootSpell()
    {
        SpellSerializationRegistry.Register(SpellWords.Shoot, spell =>
        {
            ShootSpell shootSpell = (ShootSpell) spell;
            ShootBufferPayload bufferElement = new ShootBufferPayload
            {
                shootForce = shootSpell.shootForce
            };

            return SpellSerializationRegistry.SerializeStruct(bufferElement);
        });
    }

    protected override SpellArgs CreateArgs()
    {
        ShootArgs myArgs = new ShootArgs{};

        return CopyTo(myArgs);
    }
    
}

public class ShootArgs : SpellArgs
{

    //TODO: add highlight particles here? ( spell visuals, particle effects )

}

public struct ShootBufferPayload : IBufferElementData
{
    
    public float shootForce;

}
