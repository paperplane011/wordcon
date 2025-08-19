using System;
using FronkonGames.TinyTween;
using NaughtyAttributes;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] _CGToShowAfterProgressBarArrived;

    public static Action OnProgressBarReset;

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
        CanvasEventBus.OnGameLoaded += HideButtons;
        CanvasEventBus.OnLevelsEnd += SetStartPos;
    }

    void OnDisable()
    {
        CanvasEventBus.OnResultsLoaded -= GoToNextPos;
        CanvasEventBus.OnGameLoaded -= HideButtons;
        CanvasEventBus.OnLevelsEnd -= SetStartPos;
    }

    void Start()
    {
        HideButtons();
        SetStartPos();
    }

    private void HideButtons()
    {
        foreach (var canvasGroup in _CGToShowAfterProgressBarArrived)
        {
            
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
        }


    }

    private void SetStartPos()
    {
        _currentPosNum = PlayerManager.Instance.GetCurrentLevelNum() % 5;
        if (_currentPosNum == 0) _currentPosNum = 5;

        Vector2 newPos = new();
        if (_currentPosNum == 2) newPos = _pos2.position;
        else if (_currentPosNum == 3) newPos = _pos3.position;
        else if (_currentPosNum == 4) newPos = _pos4.position;
        else if (_currentPosNum == 5) newPos = _pos5.position;
        else if (_currentPosNum == 1) newPos = _pos1.position;

        _selectionTransform.position = newPos;


    }



    private void GoToNextPos()
    {
        foreach (var canvasGroup in _CGToShowAfterProgressBarArrived)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
        }

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
        OnProgressBarReset?.Invoke();
        _selectionTransform.TweenMove(_pos1.position, TweenSettings.Instance.ProgressBarResetTime, Ease.Quint).OnEnd(tween => ProgressBarArrived());
    }

    private void ShowButtons()
    {
        TweenFloat.Create()
        .Origin(0f)
        .Destination(1f)
        .Duration(TweenSettings.Instance.NextLevelButtonFadeInTime)
        .Easing(Ease.Quint)
        .OnUpdate(tween =>
        {
            foreach (var canvasGroup in _CGToShowAfterProgressBarArrived)
            {

                canvasGroup.alpha = tween.Value;
            }
        }) 
        .OnEnd(tween =>
        {
            foreach (var canvasGroup in _CGToShowAfterProgressBarArrived)
            {

                canvasGroup.interactable = true;
            }
        })
        .Start();
    }

    private void ProgressBarArrived()
    {
        ShowButtons();

    }





}
