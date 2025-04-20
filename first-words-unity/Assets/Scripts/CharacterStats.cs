using Unity.Entities;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    private float movementSpeed;

    private class Baker : Baker<CharacterStats>
    {
        public override void Bake(CharacterStats authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CharacterStatsData
            {
                movementSpeed = authoring.movementSpeed
            });
        }
    }

}

public struct CharacterStatsData : IComponentData
{

    public float movementSpeed;
    
}
