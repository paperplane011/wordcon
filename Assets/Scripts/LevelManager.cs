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

    void OnEnable()
    {
        CanvasEventBus.OnLevelsEnd += ShuffleLevels;
    }

    void OnDisable()
    {
        CanvasEventBus.OnLevelsEnd -= ShuffleLevels;
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

    private void ShuffleLevels()
    {
        Shuffle(_levelsList);
    }

    public void Shuffle(List<LevelSO> list)
    {
        if (list == null || list.Count <= 1) return;
        
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            
            // Меняем элементы местами
            LevelSO temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
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
                if (!levelSO.IsLevelCorrect())
                {
                    Debug.LogError($"Level {levelSO.LevelNum} incorrect: ");
                }
                Debug.Log("Level added: " + levelSO.LevelNum);
            }
        }

        _levelsList.Sort(CompareLevelsByLevelNum);
        
#endif
    }


    private static int CompareLevelsByLevelNum(LevelSO x, LevelSO y)
    {
        if (x.LevelNum < y.LevelNum)
        {
            return -1;
        }
        else if (x.LevelNum > y.LevelNum)
        {
            return 1;
        }
        else
        {
            return 0;
        }
        
    }
    


    



}