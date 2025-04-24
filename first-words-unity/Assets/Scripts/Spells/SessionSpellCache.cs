using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class SessionSpellCache
{

    private static Dictionary<string, Spell> _spells = new();
    private static Dictionary<string, AsyncOperationHandle<Spell>> _handles = new();
    private static bool _isCacheReady = false;

    //NOTE for now, this is just called in an awake, but once we dynamically change the active spell based on the session,
    //we can call this while each map is loading
    public static async void LoadSessionSpells(List<SpellWords> activeSpells)
    {
        foreach(SpellWords spellWord in activeSpells)
        {
            await LoadSpell(spellWord);
        }

        _isCacheReady = true;
    }

    public static async Task<bool> LoadSpell(SpellWords spellWord)
    {
        string address = spellWord.ToString();

        if(_spells.ContainsKey(address))
        {
            return true;
        }

        var handle = Addressables.LoadAssetAsync<Spell>(address);
        await handle.Task;

        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            _spells[address] = handle.Result;
            _handles[address] = handle;
            Debug.Log($"Loaded spell: {address}");
            return true;
        }
        else
        {
            Debug.LogError($"Failed to load spell: {address}");
            return false;
        }
    }

    public static void CastSpell(SpellWords spellWord)
    {
        var spell = GetSpell(spellWord);
        spell?.Cast();  
    }

    public static Spell GetSpell(SpellWords spellWord)
    {
        string address = spellWord.ToString();

        if(_spells.TryGetValue(address, out var spell))
        {
            return spell;
        }

        Debug.LogError($"Spell not found: {address}");
        return null;
    }

    public static void UnloadAll()
    {
        foreach(var handle in _handles.Values)
        {
            Addressables.Release(handle);
        }

        _spells.Clear();
        _handles.Clear();
    }

    public static List<Spell> GetAllLoadedSpells()
    {
        return new List<Spell>(_spells.Values);
    }

    public static bool IsSpellCacheReady()
    {
        return _isCacheReady;
    }
    
}
