using System;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{

    [SerializeField]
    private int _currentLevel; // last played level, needs to be saved in cloud

    public static Action OnFiveLevelsPassed;

    private static PlayerManager _instance;

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<PlayerManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(PlayerManager).Name);
                    _instance = singletonObject.AddComponent<PlayerManager>();
                }
            }
            return _instance;
        }
    }



    private void Awake()
    {
        // Ensure only one instance exists
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        _currentLevel = 1;
    }

    

    void OnEnable()
    {
        CanvasEventBus.OnGameEnd += GameEndBehaviour;
    }

    void OnDisable()
    {
        CanvasEventBus.OnGameEnd -= GameEndBehaviour;
    }


    public int GetCurrentLevelNum()
    {
        return _currentLevel;
    }

    private void GameEndBehaviour()
    {
        _currentLevel++;

        if ((_currentLevel-1) % 5 == 0)
        {
            OnFiveLevelsPassed?.Invoke();
        }
    }


}