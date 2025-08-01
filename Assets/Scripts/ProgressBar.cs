using FronkonGames.TinyTween;
using NaughtyAttributes;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private CanvasGroup _nextLevelButtonCanvasGroup;
    [SerializeField] private BackgroundChanger _backgroundChanger;

    [BoxGroup("Selection")]
    [SerializeField] private RectTransform _selectionTransform;
    [BoxGroup("Selection")]
    [SerializeField] private RectTransform _pos1;
    [BoxGroup("Selection")]
    [SerializeField] private RectTransform _pos2;
    [BoxGroup("Selection")]
    [SerializeField] private RectTransform _pos3;
    [BoxGroup("Selection")]
    [SerializeField] private RectTransform _pos4;
    [BoxGroup("Selection")]
    [SerializeField] private RectTransform _pos5;



    private int _currentPosNum = 1;

    void OnEnable()
    {
        CanvasEventBus.OnResultsLoaded += GoToNextPos;
        CanvasEventBus.OnGameLoaded += HideNextLevelButton;
    }

    void OnDisable()
    {
        CanvasEventBus.OnResultsLoaded -= GoToNextPos;
        CanvasEventBus.OnGameLoaded -= HideNextLevelButton;
    }

    void Start()
    {
        HideNextLevelButton();
    }

    private void HideNextLevelButton()
    {

        _nextLevelButtonCanvasGroup.alpha = 0f;
        _nextLevelButtonCanvasGroup.interactable = false;

    }



    private void GoToNextPos()
    {
        _nextLevelButtonCanvasGroup.interactable = false;
        _nextLevelButtonCanvasGroup.alpha = 0f;

        Vector2 newPos = new();

        _currentPosNum++;

        if (_currentPosNum == 2) newPos = _pos2.position;
        else if (_currentPosNum == 3) newPos = _pos3.position;
        else if (_currentPosNum == 4) newPos = _pos4.position;
        else if (_currentPosNum == 5) newPos = _pos5.position;
        else if (_currentPosNum == 1) newPos = _pos1.position;

        if (_currentPosNum == 6)
        {
            ResetProgressBar();
            return;
        }

        
        _selectionTransform.TweenMove(newPos, TweenSettings.Instance.ProgressBarGoToNextPosTime, Ease.Back).EasingIn(Ease.Quint).OnEnd(tween => ProgressBarArrived());



    }

    private void ResetProgressBar()
    {
        _currentPosNum = 1;
        _selectionTransform.TweenMove(_pos1.position, TweenSettings.Instance.ProgressBarResetTime, Ease.Quint).OnEnd(tween => ProgressBarArrived(true));
    }

    private void ShowNextLevelButton()
    {
        TweenFloat.Create()
        .Origin(0f)
        .Destination(1f)
        .Duration(TweenSettings.Instance.NextLevelButtonFadeInTime)
        .Easing(Ease.Quint)
        .OnUpdate(tween => _nextLevelButtonCanvasGroup.alpha = tween.Value)
        .OnEnd(tween => _nextLevelButtonCanvasGroup.interactable = true)
        .Start();
    }

    private void ProgressBarArrived(bool endOfProgressBar = false)
    {
        ShowNextLevelButton();
        

        if(endOfProgressBar) _backgroundChanger.SetNextBackground();
    }





}
