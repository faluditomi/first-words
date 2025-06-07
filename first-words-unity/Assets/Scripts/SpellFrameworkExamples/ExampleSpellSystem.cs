using Unity.Burst;
using Unity.Entities;

/// <summary>
/// This is an example for a Burst compatible ISystem which can react to the casting of spells. In order
/// for it to be able to pick up on spells, we have to make sure it runs before the SpellEventCleanupManager.
/// </summary>
[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ExampleSpellSystem : ISystem
{
    
    private bool isInitialised;
    
    /// <summary>
    /// The OnCreate method is similar to the Awake method in MonoBehaviour. It gets called once, when
    /// the system is created.
    /// </summary>
    public void OnCreate(ref SystemState state)
    {
        //NOTE: This way we can make sure the system doesn't run unless there is at least one object in the
        // world to which this system is relevant.
        state.RequireForUpdate<ExampleSpellData>();
        isInitialised = false;
    }

    /// <summary>
    /// Like the Update method in MonoBehaviour, this runs every frame.
    /// </summary>
    public void OnUpdate(ref SystemState state)
    {
        Initialisation();
        SpellCastListener(ref state);
    }
    
    /// <summary>
    /// Called once when the game shuts down or the world gets destroyed.
    /// </summary>
    public void OnDestroy(ref SystemState state)
    {
        isInitialised = false;
    }
    
    /// <summary>
    /// Since OnCreate gets called before any MonoBehaviours could run, we have to do reference based 
    /// initialisations on the first frame of the OnUpdate, like this.
    /// </summary>
    public void Initialisation()
    {
        if(!isInitialised)
        {
            isInitialised = true;
        }
    }

    /// <summary>
    /// I order to be able to react to the casting of spells, once a system is "subscribed" to a spell in
    /// it's corresponding authoring, it has to query the SpellCastBufferElements each frame and see whether
    /// the "event" is relevant to it or not.
    /// </summary>
    public void SpellCastListener(ref SystemState state)
    {
        foreach(DynamicBuffer<SpellCastBufferElement> spellCastEventBuffer
        in SystemAPI.Query<DynamicBuffer<SpellCastBufferElement>>())
        {
            foreach(SpellCastBufferElement spellCast in spellCastEventBuffer)
            {
                foreach((RefRO<SpellListenerTag> SpellListenerTag, RefRO<ExampleSpellData> pebbleData, Entity entity)
                in SystemAPI.Query<RefRO<SpellListenerTag>, RefRO<ExampleSpellData>>().WithEntityAccess())
                {
                    if(!BurstSystemUtils.ContainsSpell(SpellListenerTag.ValueRO.listeningSpells, spellCast.spellWord))
                    {
                        continue;
                    }

                    if(spellCast.spellWord == SpellWords.Example_One)
                    {
                        ExampleComplexBufferPayload args = SpellSerializationRegistry.DeserializeStruct<ExampleComplexBufferPayload>(spellCast.payload);
                        //NOTE: Here the Example One spell behaviour can be called
                    }

                    if(spellCast.spellWord == SpellWords.Example_Two)
                    {
                        ExampleComplexBufferPayload args = SpellSerializationRegistry.DeserializeStruct<ExampleComplexBufferPayload>(spellCast.payload);
                        //NOTE: Here the Example Two spell behaviour can be called
                    }
                }
            }
        }
    }

}
