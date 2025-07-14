using System.Collections.Generic;
using UnityEngine;


public struct CanvasInfo
{
    public CanvasType CanvasGroupToLoad;
    public LoadCanvasMode Mode;
}


public enum CanvasType
{
    MainMenu,
    Game,
    Results,
    LevelsEndInfo
}

public enum LoadCanvasMode
{
    Single,
    Additive
}


public class CanvasManager : MonoBehaviour
{
    #region Singletone
    private static CanvasManager _instance;

    public static CanvasManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<CanvasManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(CanvasManager).Name);
                    _instance = singletonObject.AddComponent<CanvasManager>();

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

    #endregion

    [SerializeField] private CanvasType _startCanvasType = CanvasType.MainMenu;
    private Stack<CanvasType> _activeCanvases = new();

    [SerializeField] private CanvasGroup _gameCanvasGroup;
    [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup _resultsCanvasGroup;
    [SerializeField] private CanvasGroup _levelsEndInfoCanvasGroup;


    private void Start()
    {
        LoadCanvasGroup(_startCanvasType);

    }


    public CanvasGroup GetCanvasGroupForCanvasType(CanvasType canvasType)
    {
        if (canvasType == CanvasType.MainMenu)
        {
            return _mainMenuCanvasGroup;
        }
        else if (canvasType == CanvasType.Game)
        {
            return _gameCanvasGroup;
        }
        else if (canvasType == CanvasType.Results)
        {
            return _resultsCanvasGroup;
        }
        else if (canvasType == CanvasType.LevelsEndInfo)
        {
            return _levelsEndInfoCanvasGroup;
        }

        return _resultsCanvasGroup;
    }


    public void LoadCanvasGroup(CanvasType canvasType, LoadCanvasMode mode = LoadCanvasMode.Single)
    {
        if (mode == LoadCanvasMode.Single)
        {
            DisableAllCanvases();
            EnableCanvasGroup(GetCanvasGroupForCanvasType(canvasType));
        }
        else
        {
            EnableCanvasGroup(GetCanvasGroupForCanvasType(canvasType));
        }

        FireEventsAfterLoading(canvasType);
        _activeCanvases.Push(canvasType);
    }

    private void FireEventsAfterLoading(CanvasType canvasType)
    {
        if (canvasType == CanvasType.MainMenu)
        {
            CanvasEventBus.OnMainMenuLoaded?.Invoke();
        }
        else if (canvasType == CanvasType.Game)
        {
            CanvasEventBus.OnGameLoaded?.Invoke();
        }
        else if (canvasType == CanvasType.Results)
        {
            CanvasEventBus.OnResultsLoaded?.Invoke();
        }
    }


    private void DisableAllCanvases()
    {
        DisableCanvasGroup(_gameCanvasGroup);
        DisableCanvasGroup(_mainMenuCanvasGroup);
        DisableCanvasGroup(_resultsCanvasGroup);
        DisableCanvasGroup(_levelsEndInfoCanvasGroup);
    }

    private void DisableCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    private void EnableCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }


    public void GameEndBehaviour()
    {
        LoadCanvasGroup(CanvasType.Results, LoadCanvasMode.Additive);
        CanvasEventBus.OnGameEnd?.Invoke();
    }

    public void OnLevelsEndBehaviour()
    {
        LoadCanvasGroup(CanvasType.LevelsEndInfo, LoadCanvasMode.Additive);
        CanvasEventBus.OnLevelsEnd?.Invoke();
    }


}