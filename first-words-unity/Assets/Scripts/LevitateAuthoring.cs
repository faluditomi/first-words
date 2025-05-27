using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class LevitateAuthoring : MonoBehaviour
{

    [Min(0f), Tooltip("The speed at which the objects fly from their spot to the gathering point in front of the player.")]
    public float gatherSpeed = 10f;
    [Min(0f), Tooltip("The intensity with which the levitating objects are moving around while at the gathering point.")]
    public float jitterIntensity = 1f;
    [Min(0f), Tooltip("The distance from the target point at which the objects stop gathering and begin jittering around.")]
    public float gatheredDistance = 3f;

    public class Baker : Baker<LevitateAuthoring>
    {
        public override void Bake(LevitateAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            //TODO: find a way to get this below reference from a central point
            Transform levitateTargetTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("LevitateTarget");
            Entity levitateTargetEntity = GetEntity(levitateTargetTransform, TransformUsageFlags.Dynamic);

            AddComponent(entity, new LevitateData
            {
                gatherSpeed = authoring.gatherSpeed,
                gatheredDistance = authoring.gatheredDistance,
                jitterIntensity = authoring.jitterIntensity,
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

public struct LevitateData : IComponentData
{
    
    public float gatherSpeed;
    public float gatheredDistance;
    public float jitterIntensity;
    public Entity levitateTarget;

}