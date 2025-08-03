using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using FronkonGames.TinyTween;
using YG;

[RequireComponent(typeof(Button), typeof(CanvasGroup))]
public class HintButton : MonoBehaviour
{
    [SerializeField] private bool _isAdHintButton;

    [SerializeField] private TextMeshProUGUI _text;
    private Button _button;
    private CanvasGroup _canvasGroup;

    public static Action OnHintWithoutAdsUsed;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(Clicked);
        if (!_isAdHintButton) CanvasEventBus.OnGameLoaded += UpdateHintAmount;
        if (!_isAdHintButton) ProgressBar.OnProgressBarReset += ShowAndIncrease;
        if (_isAdHintButton) YG2.onCloseRewardedAdv += SquareManager.Instance.ShowRandomWord;
    }

    void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
        if (!_isAdHintButton) CanvasEventBus.OnGameLoaded -= UpdateHintAmount;
        if (!_isAdHintButton) ProgressBar.OnProgressBarReset -= ShowAndIncrease;
        if (_isAdHintButton) YG2.onCloseRewardedAdv -= SquareManager.Instance.ShowRandomWord;
    }


    private void UpdateHintAmount()
    {
        _text.text = PlayerManager.Instance.GetHintAmount().ToString();
        _canvasGroup.interactable = true;
        _canvasGroup.ignoreParentGroups = false;
    }


    private void Clicked()
    {
        if (_isAdHintButton) ClickedWithAds();
        else ClickedWithoutAds();

    }

    private void ClickedWithAds()
    {

        YG2.RewardedAdvShow("0");

    }

    private void ClickedWithoutAds()
    {
        if (PlayerManager.Instance.GetHintAmount() <= 0)
        {
            SoundManager.Instance.Play(SoundManager.SoundInfoName.wordNotGuessed, 0.8f);

            TweenColor.Create()
            .Origin(Color.white)
            .Destination(Color.red)
            .Duration(0.6f)
            .Loop(TweenLoop.YoYo)
            .Condition(tween => tween.ExecutionCount < 2)
            .Easing(Ease.Circ)
            .OnUpdate(tween => _text.color = tween.Value)
            .OnEnd(tween => _text.color = Color.white)
            .Start();

            return;
        }

        SquareManager.Instance.ShowRandomWord();
        OnHintWithoutAdsUsed?.Invoke();
        UpdateHintAmount();
    }


    private void ShowAndIncrease()
    {
        _text.text += "+1";
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.ignoreParentGroups = true;

        TweenFloat.Create()
        .Origin(0f)
        .Destination(1f)
        .Easing(Ease.Quint)
        .Duration(TweenSettings.Instance.ProgressBarResetTime)
        .OnUpdate(tween => _canvasGroup.alpha = tween.Value)
        .OnEnd(tween => _text.text = PlayerManager.Instance.GetHintAmount().ToString())
        .Start();
        
    }
    
    









}
