using UnityEngine;

//NOTE
// this is an example class for later on how we can create spells that require unique input arguments
[CreateAssetMenu(fileName = "ExampleComplexSpell", menuName = "Spells/ExampleComplexSpell")]
public class ExampleComplexSpell : Spell
{

    public int someProperty;

    protected override SpellArgs CreateArgs()
    {
        ExampleComplexArgs myArgs = new ExampleComplexArgs
        {
            someProperty = this.someProperty
        };

        return CopyTo(myArgs);
    }

    //NOTE
    // for complex spells, here we can also include calculations, logic, or even other services, if needed

}

public class ExampleComplexArgs : SpellArgs
{

    public int someProperty;

}
