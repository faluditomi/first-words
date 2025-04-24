using System.Collections;
using UnityEngine;

/*  <summary>  
    this is a utility class that centralises the responsibility of subscribing to spell events 
    but only once the spell cache has been populated.
    </summary> */
public class SpellEventSubscriber : MonoBehaviour
{

    public static SpellEventSubscriber Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SubscribeToSpell<TArgs>(SpellWords spellWord, System.Action<TArgs> action, System.Action<Spell<TArgs>> onSpellLoaded) where TArgs : SpellArgs
    {
        StartCoroutine(SubscribeToSpellBehaviour(spellWord, action, onSpellLoaded));
    }

    private IEnumerator SubscribeToSpellBehaviour<TArgs>(SpellWords spellWord, System.Action<TArgs> action, System.Action<Spell<TArgs>> onSpellLoaded) where TArgs : SpellArgs
    {
        yield return new WaitUntil(() => SessionSpellCache.IsSpellCacheReady());
        Spell<TArgs> spell = SessionSpellCache.GetSpell<TArgs>(spellWord);

        if (spell != null)
        {
            spell.cast += (System.Action<SpellArgs>)(object)action;
        }

        onSpellLoaded?.Invoke(spell);
    }

}
