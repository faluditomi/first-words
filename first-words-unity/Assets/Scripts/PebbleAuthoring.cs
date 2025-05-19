using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class PebbleAuthoring : MonoBehaviour
{

    //TODO: add a "size" property that will influence the gather speed and the jitter speed of the object (small things jitter and gather faster, vice versa)
    public float scaleDifferencePercentageWindow = 0.5f;

    public class Baker : Baker<PebbleAuthoring>
    {
        public override void Bake(PebbleAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            int intScaleDifference = Mathf.RoundToInt(authoring.scaleDifferencePercentageWindow * 100f);
            //TODO: find a way to get this below reference from a central point
            Transform levitateTargetTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("LevitateTarget");
            Entity levitateTargetEntity = GetEntity(levitateTargetTransform, TransformUsageFlags.Dynamic);

            AddComponent(entity, new PebbleData
            {
                scaleDifferencePercentageWindow = authoring.scaleDifferencePercentageWindow,
                randomScaleModifier = UnityEngine.Random.Range(-intScaleDifference, intScaleDifference) / 100f,
                levitateTarget = levitateTargetEntity
            });

            AddComponent(entity, new SpellListenerTag
            {
                listeningSpells = new FixedList32Bytes<SpellWords> 
                {
                    SpellWords.Levitate
                }
            });
        }
    }

}

public struct PebbleData : IComponentData
{
    
    public float scaleDifferencePercentageWindow;
    public float randomScaleModifier;
    public Entity levitateTarget;

}