using Unity.Entities;
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
            AddComponent(entity, PhysicsVelocity.Zero);
        }
    }

}
