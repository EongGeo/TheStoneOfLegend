using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<string, object> pools = new Dictionary<string, object>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CreatePool<T>(T prefab, int initCount, Transform parent = null) where T : MonoBehaviour
    {
        if (prefab == null) return;

        string key = prefab.name;
        if (pools.ContainsKey(key)) return;

        pools.Add(key, new ObjectPool<T>(prefab, initCount, parent));
    }

    public T GetFromPool<T>(T prefab) where T : MonoBehaviour
    {
        if (prefab == null) return null;

        if (!pools.TryGetValue(prefab.name, out var box))
        {
            return null;
        }

        var pool = box as ObjectPool<T>;

        if (pool != null)
        {
            return pool.Dequeque();
        }
        else
        {
            return null;
        }
    }

    public void ReturnPool<T>(T instance) where T : MonoBehaviour
    {
        if (instance == null) return;

        if (!pools.TryGetValue(instance.gameObject.name, out var box))
        {
            Destroy(instance.gameObject);
            return;
        }

        var pool = box as ObjectPool<T>;

        if (pool != null)
        {
            pool.Enqueue(instance);
        }
    }
}

internal class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab;
    private Queue<T> pool = new Queue<T>();

    public Transform Root { get; private set; }

    public ObjectPool(T prefab, int initCount, Transform parent = null)
    {
        this.prefab = prefab;
        Root = new GameObject($"{prefab.name}_pool").transform;
        Object.DontDestroyOnLoad(Root.gameObject);

        if (parent != null)
        {
            Root.SetParent(parent, false);
        }

        for (int i = 0; i < initCount; i++)
        {
            var inst = Object.Instantiate(prefab, Root);
            inst.name = prefab.name;
            inst.gameObject.SetActive(false);
            pool.Enqueue(inst);
        }
    }
    public T Dequeque()
    {
        if (pool.Count == 0) return null;
        var inst = pool.Dequeue();
        inst.gameObject.SetActive(true);
        return inst;
    }

    public void Enqueue(T instance)
    {
        if (instance == null) return;

        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);
    }

}
