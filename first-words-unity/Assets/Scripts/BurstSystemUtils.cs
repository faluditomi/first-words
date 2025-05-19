using Unity.Collections;

/// <summary>
/// A utility class to hold all the helper methods that Burst compatible systems use regularly.
/// </summary>
public struct BurstSystemUtils
{

    public static bool ContainsSpell(in FixedList32Bytes<SpellWords> listeningSpells, SpellWords spellWord)
    {
        for(int i = 0; i < listeningSpells.Length; i++)
        {
            if(listeningSpells[i] == spellWord)
            {
                return true;
            }
        }

        return false;
    }

}
