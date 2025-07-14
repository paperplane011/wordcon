using UnityEngine;


public class PlayerManager : MonoBehaviour
{

    [SerializeField]
    private int _lastLevelNum; // last played level, needs to be saved in cloud

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
    }

    void OnEnable()
    {
        CanvasEventBus.OnGameEnd += GameEndBehaviour;
    }

    void OnDisable()
    {
        CanvasEventBus.OnGameEnd -= GameEndBehaviour;
    }


    public int GetLastLevelNum()
    {
        return _lastLevelNum;
    }

    private void GameEndBehaviour()
    {
        _lastLevelNum++;
    }


}