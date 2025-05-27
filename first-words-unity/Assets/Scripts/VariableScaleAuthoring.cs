using Unity.Entities;
using Unity.Entities.UI;
using UnityEngine;

public class VariableScaleAuthoring : MonoBehaviour
{
    
    [MinMax(0f, 100f), Tooltip("The maximum magnitude of the scale change randomisation in percentage.")]
    public float scaleDifferencePercentageWindow = 50f;

    public class Baker : Baker<VariableScaleAuthoring>
    {
        public override void Bake(VariableScaleAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            int intScaleDifference = Mathf.RoundToInt(authoring.scaleDifferencePercentageWindow);

            AddComponent(entity, new VariableScaleData
            {
                randomScaleModifier = UnityEngine.Random.Range(-intScaleDifference, intScaleDifference) / 100f
            });
        }
    }

}

public struct VariableScaleData : IComponentData
{
    
    public float randomScaleModifier;

}
