using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Managers
{
    private static GameObject _root;
    private static SoundManager _sound;

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
    public static SoundManager Sound
    {
        get
        {
            CreateManager(ref _sound, "SoundManager");
            return _sound;
        }
    }
}
