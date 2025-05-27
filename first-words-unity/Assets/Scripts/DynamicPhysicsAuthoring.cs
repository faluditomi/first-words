using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class DynamicPhysicsAuthoring : MonoBehaviour
{
    
    public class Baker : Baker<DynamicPhysicsAuthoring>
    {
        public override void Bake(DynamicPhysicsAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PhysicsMass>(entity);
            //TODO: this is messing with the visibility of the pebbles
            AddComponent(entity, PhysicsVelocity.Zero);
        }
    }

}
