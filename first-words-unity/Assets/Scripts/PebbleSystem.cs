using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct PebbleSystem : ISystem
{
    private bool isInitialised;
    
    public void OnCreate(ref SystemState state)
    {
        //Make sure the system doesn't run unless there is at least one pebble in the world
        state.RequireForUpdate<PebbleData>();
        isInitialised = false;
    }

    public void OnUpdate(ref SystemState state)
    {
        if(!isInitialised)
        {
            foreach((RefRO<PebbleData> pebbleData, RefRW<LocalTransform> localTransform) 
                in SystemAPI.Query<RefRO<PebbleData>, RefRW<LocalTransform>>())
            {
                localTransform.ValueRW = localTransform.ValueRW.WithScale(localTransform.ValueRO.Scale * (1f + pebbleData.ValueRO.randomScaleModifier));
            }

            isInitialised = true;
        }
    }

}
