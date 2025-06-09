using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using static GameplayTags;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ShootSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        //TODO: should probably beef up this check and look for more components
        state.RequireForUpdate<ShootData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        SpellCastListener(ref state);
        //TODO: query for all shooting objects and with some kind of timer solution, remove the shooting tag after a second or so (so it cannot be relevitated right after being shot)
            //TODO: also remove the tag if they hit smth
    }

    private void SpellCastListener(ref SystemState state)
    {
        //REVIEW: can there be multiple DynamicBuffers? if not, we can remove this first foreach in all systems taht listen to spells
        foreach(DynamicBuffer<SpellCastBufferElement> spellCastEventBuffer
        in SystemAPI.Query<DynamicBuffer<SpellCastBufferElement>>())
        {
            foreach(SpellCastBufferElement spellCast in spellCastEventBuffer)
            {
                foreach(
                    (RefRO<SpellListenerTag> spellListenerTag,
                    RefRO<ShootData> ShootData,
                    RefRW<PhysicsVelocity> velocity,
                    RefRO<PhysicsMass> mass,
                    Entity entity)
                in SystemAPI.Query<
                    RefRO<SpellListenerTag>,
                    RefRO<ShootData>,
                    RefRW<PhysicsVelocity>,
                    RefRO<PhysicsMass>>()
                    .WithAll<LevitatingTag>()
                    .WithNone<ShootingTag>()
                    .WithEntityAccess())
                {
                    if(!BurstSystemUtils.ContainsSpell(spellListenerTag.ValueRO.listeningSpells, spellCast.spellWord))
                    {
                        continue;
                    }

                    if(spellCast.spellWord == SpellWords.Shoot)
                    {
                        ShootBufferPayload args = SpellSerializationRegistry.DeserializeStruct<ShootBufferPayload>(spellCast.payload);
                        state.EntityManager.RemoveComponent<LevitatingTag>(entity);
                        state.EntityManager.AddComponent<ShootingTag>(entity);
                        
                        float3 moveVector = SystemAPI.GetComponentRO<LocalTransform>(ShootData.ValueRO.levitateTarget).ValueRO.Forward();
                        moveVector = math.normalizesafe(moveVector);
                        //REVIEW: if we want all objects to shoot at the same speed, i think we can remove the mass from the equiation
                        float3 force = moveVector * args.shootForce * mass.ValueRO.InverseMass;

                        velocity.ValueRW.Linear += force;
                    }
                }
            }
        }
    }

}
