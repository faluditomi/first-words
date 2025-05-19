/// <summary>
/// The collection of all spell-words that are used in the game. This list is used to load the spells from the addressables system
/// and to prevent missplelling related errors. Name the spells as they are to be pronounced. Always use "_" instead of " " in the name.
/// </summary>
public enum SpellWords
{

    Levitate,

    #region Whisper Performance Test Spells
    Forward,
    Back,
    Left,
    Right,
    Jump,
    Stop,
    Switch_Cam,
    Reset,
    Fuck,
    #endregion

    #region Example Spells
    Example_One,
    Example_Two
    #endregion

}
