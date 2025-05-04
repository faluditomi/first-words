using UnityEngine;

[CreateAssetMenu(fileName = "MoveBallSpell", menuName = "Spells/MoveBallSpell")]
public class MoveBallSpell : Spell
{
    
    public Vector3 direction;
    public float strength;

    protected override SpellArgs CreateArgs()
    {
        MoveBallArgs myArgs = new MoveBallArgs
        {
            direction = this.direction.normalized,
            strength = this.strength
        };

        return CopyTo(myArgs);
    }

}

public class MoveBallArgs : SpellArgs
{

    public Vector3 direction;
    public float strength;

}
