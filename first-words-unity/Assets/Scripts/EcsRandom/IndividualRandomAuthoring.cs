using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class IndividualRandomAuthoring : MonoBehaviour
{

    public class Baker : Baker<IndividualRandomAuthoring>
    {
        public override void Bake(IndividualRandomAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            uint seed = (uint) entity.Index + 1;
            AddComponent(entity, new IndividualRandomData
            {
                value = Random.CreateFromIndex(seed)
            });
        }
    }

}

public struct IndividualRandomData : IComponentData
{

    public Random value;

}
