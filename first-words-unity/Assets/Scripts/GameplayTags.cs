using Unity.Entities;

/// <summary>
/// Used in ISystems to keep track of the state of Entities. Tags are empty components we query for in 
/// Burst-compatible systems.
/// </summary>
public struct GameplayTags
{

    public struct LevitatingTag : IComponentData { }
    public struct ShootingTag : IComponentData { }

}
