using UnityEngine;

//NOTE
// this is an example class for later on how we can create spells that require unique input arguments
[CreateAssetMenu(fileName = "ExampleComplexSpell", menuName = "Spells/ExampleComplexSpell")]
public class ExampleComplexSpell : Spell
{

    public int someProperty;
    
    protected override SpellEventArgs CreateArgs()
    {
        return new ExampleComplexArgs
        {
            someProperty = this.someProperty
        };
    }

    //NOTE
    // for complex spells, here we can also include calculations, logic, or even other services, if needed

}

public class ExampleComplexArgs : SpellEventArgs
{

    public int someProperty;

}
