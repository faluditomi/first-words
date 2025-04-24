using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSpell", menuName = "Spells/DefaultSpell")]
public class DefaultSpell : Spell<SpellArgs>
{
    protected override SpellArgs CreateArgs()
    {
        return new SpellArgs();
    }
}
