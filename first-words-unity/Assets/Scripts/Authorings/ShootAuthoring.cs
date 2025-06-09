using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class ShootAuthoring : MonoBehaviour
{

    public class Baker : Baker<ShootAuthoring>
    {
        public override void Bake(ShootAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            //TODO: find a way to get this below reference from a central point
            Transform levitateTargetTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("LevitateTarget");
            Entity levitateTargetEntity = GetEntity(levitateTargetTransform, TransformUsageFlags.Dynamic);

            AddComponent(entity, new ShootData
            {
                levitateTarget = levitateTargetEntity
            });

            AddComponent(entity, new SpellListenerTag
            {
                listeningSpells = new FixedList32Bytes<SpellWords>
                {
                    SpellWords.Shoot
                }
            });
        }
    }

}

public struct ShootData : IComponentData
{

    public Entity levitateTarget;

}
