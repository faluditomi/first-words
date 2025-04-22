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

    public void SubscribeToSpell(SpellWords spellWord, System.Action<SpellEventArgs> action, System.Action<Spell> onSpellLoaded)
    {
        StartCoroutine(SubscribeToSpellBehaviour(spellWord, action, onSpellLoaded));
    }

    private IEnumerator SubscribeToSpellBehaviour(SpellWords spellWord, System.Action<SpellEventArgs> action, System.Action<Spell> onSpellLoaded)
    {
        Debug.Log("STARED SUBSCRIPTION PROCESS");
        yield return new WaitUntil(() => SessionSpellCache.IsSpellCacheReady());
        Debug.Log("WAITED UNTIL CACHE READY");
        Spell spell = SessionSpellCache.GetSpell(spellWord);
        Debug.Log($"FOUND SPELL: {spell}");

        if (spell != null)
        {
            spell.cast += action;
        }

        onSpellLoaded?.Invoke(spell);
        Debug.Log("INVOKED EVENT");
    }
}
