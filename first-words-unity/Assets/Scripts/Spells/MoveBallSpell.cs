using UnityEngine;

[CreateAssetMenu(fileName = "MoveBallSpell", menuName = "Spells/MoveBallSpell")]
public class MoveBallSpell : Spell<MoveBallArgs>
{
    
    public Vector3 direction;
    public float strength;

    protected override MoveBallArgs CreateArgs()
    {
        return new MoveBallArgs
        {
            direction = this.direction.normalized,
            strength = this.strength
        };
    }

}

public class MoveBallArgs : SpellArgs
{

    public Vector3 direction;
    public float strength;

}
