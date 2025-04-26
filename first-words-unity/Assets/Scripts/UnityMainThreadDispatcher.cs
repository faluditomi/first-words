using System;
using System.Collections.Concurrent;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{

    private static UnityMainThreadDispatcher _instance;
    private static readonly ConcurrentQueue<Action> _executionQueue = new();

    //NOTE
    // this singleton template is great for scripts that we want to persist across scenes
    public static UnityMainThreadDispatcher Instance()
    {
        if(_instance == null)
        {
            var obj = new GameObject("UnityMainThreadDispatcher");
            _instance = obj.AddComponent<UnityMainThreadDispatcher>();
            DontDestroyOnLoad(obj);
        }

        return _instance;
    }

    public void Enqueue(Action action)
    {
        if(action == null) throw new ArgumentNullException(nameof(action));
        _executionQueue.Enqueue(action);
    }

    private void Update()
    {
        while(_executionQueue.TryDequeue(out var action))
        {
            action.Invoke();
        }
    }

}