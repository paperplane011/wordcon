#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;


public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    [SerializeField]
    private List<LevelSO> _levelsList;


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

    public bool TryGetLevelSOByLevelNum(int levelNum, out LevelSO levelSO)
    {
        levelNum--;

        if (levelNum >= _levelsList.Count)
        {
            levelSO = null;
            return false;
        }

        levelSO = _levelsList[levelNum];
        return true;
    }

    public int GetLevelCount()
    {
        return _levelsList.Count;
    }





    [Button("Fill Levels List")]
    private void GetAllLevelSOs()
    {
        _levelsList = new();

#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets("t:LevelSO");

        foreach (string guid in guids)
        {

            string path = AssetDatabase.GUIDToAssetPath(guid);
            LevelSO levelSO = AssetDatabase.LoadAssetAtPath<LevelSO>(path);
            if (levelSO != null)
            {
                _levelsList.Add(levelSO);
                Debug.Log("level added: " + levelSO.name);
            }
        }
#endif
    }
    


    



}