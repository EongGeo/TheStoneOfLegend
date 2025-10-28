using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers
{
    private static GameObject _root;
    private static GameManager _game;
    private static StageManager _stage;
    private static PoolManager _pool;
    private static SpawnManager _spawn;

    private static void Init()
    {
        if (_root == null)
        {
            _root = new GameObject("@Managers");
            Object.DontDestroyOnLoad(_root);
        }
    }
    private static void CreateManager<T>(ref T manager, string name) where T : Component
    {
        if (manager == null)
        {
            Init();

            GameObject obj = new GameObject(name);

            manager = obj.AddComponent<T>();

            Object.DontDestroyOnLoad(obj);

            obj.transform.SetParent(_root.transform);
        }
    }
    public static GameManager Game
    {
        get
        {
            CreateManager(ref _game, "GameManager");
            return _game;
        }
    }
    public static StageManager Stage
    {
        get
        {
            CreateManager(ref _stage, "StageManager");
            return _stage;
        }
    }
    public static PoolManager Pool
    {
        get
        {
            CreateManager(ref _pool, "PoolManager");
            return _pool;
        }
    }
    public static SpawnManager Spawn
    {
        get
        {
            CreateManager(ref _spawn, "SpawnManager");
            return _spawn;
        }
    }
}
