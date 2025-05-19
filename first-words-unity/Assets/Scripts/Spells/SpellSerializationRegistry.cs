using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

/// <summary>
/// A helper class used to serialize and deserialize the properties of specific spells, such that
/// they can be added to the SpellCastBufferElement in the SpellEventManager. This allows for a 
/// generalised solution and makes it possible to pass data to the systems without losing 
/// Burst compatibility.
/// </summary>
public static class SpellSerializationRegistry
{

    public delegate FixedBytes126 SpellArgsSerializer(Spell spell);
    private static readonly Dictionary<SpellWords, SpellArgsSerializer> serializers = new();

    public static void Register(SpellWords spellWord, SpellArgsSerializer serializer)
    {
        serializers[spellWord] = serializer;
    }

    public static FixedBytes126 Serialize(SpellWords spellWord, Spell spell)
    {
        if(serializers.TryGetValue(spellWord, out var serializer))
        {
            return serializer(spell);
        }

        return default;
    }

    /// <summary>
    /// Used by specific spells to serialize their BufferPayloads such that it can be added to
    /// the SpellCastBufferElement.
    /// </summary>
    /// <typeparam name="T">The type of the specific BufferPayload we are passing</typeparam>
    /// <param name="value">The BufferPayload</param>
    /// <returns>The BufferPayload as bytes</returns>
    public static FixedBytes126 SerializeStruct<T>(T value) where T : unmanaged
    {
        FixedBytes126 bytes = default;

        unsafe
        {
            UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref bytes), &value, sizeof(T));
        }

        return bytes;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the specific BufferPayload we want to receive</typeparam>
    /// <param name="value">The bytes from the SpellCastBufferElement payload</param>
    /// <returns>The BufferPayload in the specified type</returns>
    public static T DeserializeStruct<T>(FixedBytes126 bytes) where T : unmanaged
    {
        T value = default;

        unsafe
        {
            UnsafeUtility.MemCpy(&value, UnsafeUtility.AddressOf(ref bytes), sizeof(T));
        }

        return value;
    }

}
