using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct VariableScaleSystem : ISystem
{

    private bool isInitialised;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<VariableScaleData>();
        isInitialised = false;
    }

    public void OnUpdate(ref SystemState state)
    {
        Initialisation(ref state);
    }

    private void Initialisation(ref SystemState state)
    {
        if(!isInitialised)
        {
            //TODO: also have to scale the colliders
            foreach((RefRO<VariableScaleData> pebbleData, RefRW<LocalTransform> localTransform)
            in SystemAPI.Query<RefRO<VariableScaleData>, RefRW<LocalTransform>>())
            {
                //REVIEW: Once we need randomisation at runtime, we might as well use Unity.Mathematics.Random for this as well
                localTransform.ValueRW = localTransform.ValueRW.WithScale(localTransform.ValueRO.Scale * (1f + pebbleData.ValueRO.randomScaleModifier));
            }

            isInitialised = true;
        }
    }

}
