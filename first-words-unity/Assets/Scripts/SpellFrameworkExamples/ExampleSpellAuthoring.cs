using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UI;
using UnityEngine;

/// <summary>
/// This is an example authoring script to showcase how we can set up a Burst compatible ISystems that
/// can react to the casting of spells. If we want an object to be controlled by a certain ISystem,
/// we must place the corresponding authoring script on it.
/// </summary>
public class ExampleSpellAuthoring : MonoBehaviour
{

    /// <summary>
    /// Here in the authoring class, we include fields that we will not only want to pass to the system,
    /// but ones that we also want to tweak from the editor.
    /// </summary>
    [MinMax(-1f, 1f), Tooltip("This is an example field on an example class. Y u usin it?")]
    public float someFloat = 1;

    /// <summary>
    /// The baker class is the bridge between the object-oriented nature of the Unity editor and the
    /// data-oriented nature of ECS. It hooks up all the entities and data which the system can later
    /// access.
    /// </summary>
    public class Baker : Baker<ExampleSpellAuthoring>
    {
        public override void Bake(ExampleSpellAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            int intScaleDifference = Mathf.RoundToInt(authoring.someFloat * 100f);
            //TODO: find a way to get this reference below from a central point
            Transform transform = GameObject.FindGameObjectWithTag("ThingA").transform.Find("ThingB");
            Entity levitateTargetEntity = GetEntity(transform, TransformUsageFlags.Dynamic);

            //NOTE: This is how we attach components to the Entity, which the system can later access and use.
            AddComponent(entity, new ExampleSpellData
            {
                someInt = UnityEngine.Random.Range(-intScaleDifference, intScaleDifference),
                someEntity = levitateTargetEntity
            });

            //NOTE: This is the equivalent of a MonoBehaviour subscribing to a Spell.
            AddComponent(entity, new SpellListenerTag
            {
                listeningSpells = new FixedList32Bytes<SpellWords>
                {
                    SpellWords.Example_One,
                    SpellWords.Example_Two
                }
            });
        }
    }

}

/// <summary>
/// The data scripts are included in the authoring scripts for convenience and simplicity. These are 
/// the properties which we want to pass to the System from here.
/// </summary>
public struct ExampleSpellData : IComponentData
{

    public int someInt;
    public Entity someEntity;

}
