using System;
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

    public void SubscribeToSpell(SpellWords spellWord, System.Action<SpellArgs> action)
    {
        StartCoroutine(SubscribeToSpellBehaviour(spellWord, action));
    }

    //NOTE
    // useful if we want to unsubscribe from events, but now we clear the spell cache on scene unload
    // so we don't need this for now
    public void UnsubscribeFromSpell(SpellWords spellWord, System.Action<SpellArgs> action)
    {
        Spell spell = SessionSpellCache.GetSpell(spellWord);

        if(spell == null)
        {
            Debug.LogWarning($"Spell {spellWord} not found in cache. Cannot unsubscribe.");
            return;
        }

        spell.cast -= action;
        Debug.Log($"Unsubscribed from {spellWord} spell event.");
    }

    private IEnumerator SubscribeToSpellBehaviour(SpellWords spellWord, System.Action<SpellArgs> action)
    {
        yield return new WaitUntil(() => SessionSpellCache.IsSpellCacheReady());
        Spell spell = SessionSpellCache.GetSpell(spellWord);
        
        if(spell == null)
        {
            Debug.LogWarning($"Spell {spellWord} not found in cache. Cannot subscribe.");
            yield break;
        }

        spell.cast += action;
        Debug.Log($"Subscribed to {spellWord} spell event.");
    }

}
