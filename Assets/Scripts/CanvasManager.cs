using System.Collections.Generic;
using FronkonGames.TinyTween;
using UnityEditor.Rendering;
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
    [SerializeField] private RectTransform _resultsPanelRectTransform;
    

    [SerializeField] private CanvasGroup _gameCanvasGroup;
    [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup _resultsCanvasGroup;
    [SerializeField] private CanvasGroup _levelsEndInfoCanvasGroup;
    

    private CanvasGroup _currentCanvasGroup;
    private CanvasType _currentCanvasType;

    


    private void Start()
    {
        DisableAllCanvases();

        _currentCanvasGroup = GetCanvasGroupForCanvasType(_startCanvasType);
        _currentCanvasType = _startCanvasType;
        _currentCanvasGroup.alpha = 1f;
        _currentCanvasGroup.interactable = true;
        _currentCanvasGroup.blocksRaycasts = true;
        FireEventsAfterLoading(_startCanvasType);
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
            EnableCanvasGroupForCanvasType(canvasType);
            _currentCanvasType = canvasType;
            FireEventsAfterLoading(canvasType);
        }
        else
        {
            EnableCanvasGroupForCanvasType(canvasType, true);
        }

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

    private void EnableCanvasGroupForCanvasType(CanvasType canvasType, bool additive = false)
    {
        if (_currentCanvasType == canvasType)
        {
            _currentCanvasGroup.alpha = 1f;
            _currentCanvasGroup.interactable = true;
            _currentCanvasGroup.blocksRaycasts = true;
            return;
        }

        CanvasGroup canvasGroup = GetCanvasGroupForCanvasType(canvasType);

        _currentCanvasGroup.interactable = false;
        _currentCanvasGroup.blocksRaycasts = false;

        TweenFloat.Create()
        .Origin(1f)
        .Destination(0f)
        .Duration((canvasType == CanvasType.Results) ? TweenSettings.Instance.ResultsCanvasFadeTime : TweenSettings.Instance.DefaultCanvasFadeTime)
        .OnUpdate(tween => { if (additive) _currentCanvasGroup.alpha = tween.Value;canvasGroup.alpha = 1f - tween.Value; })
        .OnEnd(tween => {  })
        .OnEnd(tween => { if (canvasType == CanvasType.Results) FireEventsAfterLoading(canvasType); })
        .Easing((canvasType == CanvasType.Results) ? Ease.Quint : Ease.Circ)
        .Start();

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        if (!additive) _currentCanvasGroup = canvasGroup;

        if (canvasType == CanvasType.Results)
        {

            TweenFloat.Create()
            .Origin(2000f) 
            .Destination(0f)
            .Easing(Ease.Quart)
            .Duration(TweenSettings.Instance.ResultsCanvasFadeTime*1.7f)
            .OnUpdate(tween => _resultsPanelRectTransform.sizeDelta = new Vector2(_resultsPanelRectTransform.sizeDelta.x, -tween.Value))
            .Start();
        }

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