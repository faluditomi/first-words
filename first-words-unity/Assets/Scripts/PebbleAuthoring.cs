using UnityEngine;
using Unity.Entities;

public class PebbleAuthoring : MonoBehaviour
{

    public float shootForce = 10f;
    public float jitterIntensity = 1f;
    public float scaleDifferencePercentageWindow = 0.5f;

    public class Baker : Baker<PebbleAuthoring>
    {
        public override void Bake(PebbleAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            int intScaleDifference = Mathf.RoundToInt(authoring.scaleDifferencePercentageWindow * 100f);

            AddComponent(entity, new PebbleData
            {
                shootForce = authoring.shootForce,
                jitterIntensity = authoring.jitterIntensity,
                scaleDifferencePercentageWindow = authoring.scaleDifferencePercentageWindow,
                randomScaleModifier = UnityEngine.Random.Range(-intScaleDifference, intScaleDifference) / 100f
            });
        }
    }

}

public struct PebbleData : IComponentData
{
    
    public float shootForce;
    public float jitterIntensity;
    public float scaleDifferencePercentageWindow;
    [HideInInspector]
    public float randomScaleModifier;

}