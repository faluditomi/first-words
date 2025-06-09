using Unity.Entities;
using Unity.Burst;

/// <summary>
/// Every frame, after all the spell related systems executed, this system comes in and clears the 
/// SpellCastBufferElements so that no system will react to the same spell twice in the next frame.
/// </summary>
[BurstCompile]
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial struct SpellEventCleanupManager : ISystem
{

    public void OnUpdate(ref SystemState state)
    {
        foreach(DynamicBuffer<SpellCastBufferElement> buffer in SystemAPI.Query<DynamicBuffer<SpellCastBufferElement>>())
        {
            buffer.Clear();
        }
    }

}