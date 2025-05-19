using Unity.Collections;
using Unity.Entities;

/// <summary>
/// A central point for the triggering/casting of spells.
/// </summary>
public struct SpellEventManager
{

    /// <summary>
    /// Finds a spell in the cache and delegates its casting to the main thread. It triggers the cast event
    /// and also puts a SpellCastBufferElement onto the buffer, so that the Burst-compatible systems can
    /// also react to the casting of spells.
    /// </summary>
    /// <param name="spellWord"> The SpellWord/Spell we want to cast. </param>
    public static void CastSpell(SpellWords spellWord)
    {
        Spell spell = SpellSessionCache.GetSpell(spellWord);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            spell?.Cast();

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity bufferEntity = GetOrCreateEventBufferEntity();
            DynamicBuffer<SpellCastBufferElement> buffer = entityManager.GetBuffer<SpellCastBufferElement>(bufferEntity);
            FixedBytes126 payload = SpellSerializationRegistry.Serialize(spellWord, spell);

            buffer.Add(new SpellCastBufferElement
            {
                spellWord = spellWord,
                payload = payload
            });
        });
    }

    private static Entity GetOrCreateEventBufferEntity()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery query = entityManager.CreateEntityQuery(typeof(SpellCastBufferElement));

        if(query.IsEmpty)
        {
            Entity entity = entityManager.CreateEntity();
            entityManager.AddBuffer<SpellCastBufferElement>(entity);
            return entity;
        }
        else
        {
            return query.GetSingletonEntity();
        }
    }

}
