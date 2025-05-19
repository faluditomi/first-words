using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(SpellEventCleanupManager))]
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
        Initialisation(ref state);
        SpellCastListener(ref state);
    }

    public void Initialisation(ref SystemState state) 
    {
        if(!isInitialised)
        {
            foreach(
                (RefRO<PebbleData> pebbleData,
                RefRW<LocalTransform> localTransform)
                in SystemAPI.Query<RefRO<PebbleData>, RefRW<LocalTransform>>())
            {
                //REVIEW: Once we need randomisation at runtime, we might as well use Unity.Mathematics.Random for this as well
                localTransform.ValueRW = localTransform.ValueRW.WithScale(localTransform.ValueRO.Scale * (1f + pebbleData.ValueRO.randomScaleModifier));
            }

            isInitialised = true;
        }
    }

    //REVIEW: can we delegate someof this to a generalised util class/method somewhere? (Maybe in the BurstSystemUtility? - Rename that to BurstSystemSpellUtility?)
    public void SpellCastListener(ref SystemState state)
    {
        foreach(DynamicBuffer<SpellCastBufferElement> spellCastEventBuffer
        in SystemAPI.Query<DynamicBuffer<SpellCastBufferElement>>())
        {
            foreach(SpellCastBufferElement spellCast in spellCastEventBuffer)
            {
                foreach((RefRO<SpellListenerTag> SpellListenerTag, RefRO<PebbleData> pebbleData, Entity entity)
                in SystemAPI.Query<RefRO<SpellListenerTag>, RefRO<PebbleData>>().WithEntityAccess())
                {
                    if(!BurstSystemUtils.ContainsSpell(SpellListenerTag.ValueRO.listeningSpells, spellCast.spellWord))
                    {
                        continue;
                    }

                    if(spellCast.spellWord == SpellWords.Levitate)
                    {
                        LevitateBufferPayload args = SpellSerializationRegistry.DeserializeStruct<LevitateBufferPayload>(spellCast.payload);
                        //TODO: spell logic
                    }
                }
            }
        }
    }

}
