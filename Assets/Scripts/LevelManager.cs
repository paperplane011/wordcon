#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    [SerializeField]
    [ReadOnly]
    private List<LevelSO> _levels;

    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<LevelManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(LevelManager).Name);
                    _instance = singletonObject.AddComponent<LevelManager>();

                }
            }
            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    
    [Button("Fill Levels List")]
    private void GetAllLevelSOs()
    {
        _levels = new List<LevelSO>();

#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets("t:LevelSO");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            LevelSO level = AssetDatabase.LoadAssetAtPath<LevelSO>(path);
            if (level != null)
            {
                _levels.Add(level);
            }
        }
#endif
    }
    



}