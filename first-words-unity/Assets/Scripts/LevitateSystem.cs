using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using static GameplayTags;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct PebbleSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        //Make sure the system doesn't run unless there is at least one pebble in the world
        //TODO: should probably beef up this check and look for more components
        state.RequireForUpdate<LevitateData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        SpellCastListener(ref state);
        LevitateBehaviour(ref state);
    }

    private void SpellCastListener(ref SystemState state)
    {
        foreach(DynamicBuffer<SpellCastBufferElement> spellCastEventBuffer
        in SystemAPI.Query<DynamicBuffer<SpellCastBufferElement>>())
        {
            foreach(SpellCastBufferElement spellCast in spellCastEventBuffer)
            {
                foreach(
                    (RefRO<SpellListenerTag> SpellListenerTag,
                    RefRO<LevitateData> levitateData,
                    RefRW<LocalTransform> localTransform,
                    Entity entity)
                in SystemAPI.Query<
                    RefRO<SpellListenerTag>,
                    RefRO<LevitateData>,
                    RefRW<LocalTransform>>()
                    .WithAbsent<ShootingTag>()
                    .WithEntityAccess())
                {
                    if(!BurstSystemUtils.ContainsSpell(SpellListenerTag.ValueRO.listeningSpells, spellCast.spellWord))
                    {
                        continue;
                    }

                    if(spellCast.spellWord == SpellWords.Levitate)
                    {
                        LevitateBufferPayload args = SpellSerializationRegistry.DeserializeStruct<LevitateBufferPayload>(spellCast.payload);
                        LevitateTagger(ref state, levitateData.ValueRO, localTransform, args, entity);
                    }
                }
            }
        }
    }

    private void LevitateTagger(
        ref SystemState state,
        LevitateData levitateData,
        RefRW<LocalTransform> localTransform,
        LevitateBufferPayload args,
        Entity entity)
    {
        float3 pebblePosition = localTransform.ValueRO.Position;
        float3 targetPosition = SystemAPI.GetComponentRO<LocalTransform>(levitateData.levitateTarget).ValueRO.Position;

        if(math.distance(pebblePosition, targetPosition) <= args.range)
        {
            state.EntityManager.AddComponent<LevitatingTag>(entity);
        }

        //TODO: SHOOTING SYSTEM (better name for it?)
        //TODO: queries for its own components and ones that don't yet have the shooting tag but have the levitate tag
        //TODO: then it puts the shooting tag on them and begins applying velocity (or force?) to them in the player's (levitateTarget's?) forward direction
        //TODO: after a certain amount of time, the shooting system lets go of the objects and removes the shooting tag (this should also happen if they hit smth)
    }

    private void LevitateBehaviour(ref SystemState state)
    {
        foreach((
            RefRO<LevitateData> levitateData,
            RefRO<LocalTransform> localTransform,
            RefRW<PhysicsVelocity> velocity,
            RefRO<PhysicsMass> mass)
        in SystemAPI.Query<
            RefRO<LevitateData>,
            RefRO<LocalTransform>,
            RefRW<PhysicsVelocity>,
            RefRO<PhysicsMass>>()
            .WithAll<LevitatingTag>())
        {
            float3 currentPosition = localTransform.ValueRO.Position;
            float3 targetPosition = SystemAPI.GetComponentRO<LocalTransform>(levitateData.ValueRO.levitateTarget).ValueRO.Position;
            float3 moveVector = targetPosition - currentPosition;
            float distance = math.length(moveVector);

            //TODO: if its closer than gatheredDistance or smth, apply random force to it to simulate jittering
            float3 forceDirection = math.normalizesafe(moveVector);
            float forceStrength = distance > levitateData.ValueRO.gatheredDistance
                ? levitateData.ValueRO.gatherSpeed 
                : levitateData.ValueRO.gatherSpeed * (distance / levitateData.ValueRO.gatheredDistance);
            float3 force = forceDirection * forceStrength * mass.ValueRO.InverseMass * SystemAPI.Time.DeltaTime;

            velocity.ValueRW.Linear += force;
        }
    }

}
