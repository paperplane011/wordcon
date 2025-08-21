using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using FronkonGames.TinyTween;
using YG;

[RequireComponent(typeof(Button), typeof(CanvasGroup))]
public class HintButton : MonoBehaviour
{
    //[SerializeField] private bool _isAdHintButton;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private CanvasGroup _adIconCanvasGroup; // show ad icon when there is no hints
    [SerializeField] private bool _isWordHint = true;

    private Button _button;
    private CanvasGroup _buttonCanvasGroup;
    

    public static Action OnHintWordUsed;
    public static Action OnHintLetterUsed;

    private bool _doesHaveHints;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _buttonCanvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(Clicked);
        ProgressBar.OnProgressBarReset += ShowAndIncrease;
        CanvasEventBus.OnGameLoaded += UpdateHintAmount;
    }

    void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
        ProgressBar.OnProgressBarReset -= ShowAndIncrease;
        CanvasEventBus.OnGameLoaded -= UpdateHintAmount;
    }

    void Start()
    {
        UpdateHintAmount();
    }


    private void UpdateHintAmount()
    {
        int newHintAmount = (_isWordHint) ? PlayerManager.Instance.GetHintWordAmount() : PlayerManager.Instance.GetHintLetterAmount();


        if (newHintAmount <= 0)
        {
            _doesHaveHints = false;
            _adIconCanvasGroup.alpha = 1f;
            if (_isWordHint)
            {
                _text.text = "+1";
            }
            else
            {
                _text.text = "+2";
            }
            
        }
        else
        {
            _doesHaveHints = true;
            _text.text = newHintAmount.ToString();
            _adIconCanvasGroup.alpha = 0f;
        }

        _buttonCanvasGroup.interactable = true;
        _buttonCanvasGroup.ignoreParentGroups = false;
    }


    private void Clicked()
    {
        if (_doesHaveHints)
        {
            if (_isWordHint)
            {
                TryToUseHintWord();
            }
            else
            {
                TryToUseHintLetter();
            }
            
        }
        else
        {
            AddHintsForAd();
        }

    }

    private void AddHintsForAd()
    {
        YG2.RewardedAdvShow("0");

        if (_isWordHint)
        {
            YG2.saves.hintAmountWord += 1;
        }
        else
        {
            YG2.saves.hintAmountLetter += 2;
        }
        
        YG2.SaveProgress();

        _buttonCanvasGroup.interactable = false;
        UpdateHintAmount();
    }

    private void TryToUseHintWord()
    {
        
        if (!SquareManager.Instance.CanShowRandomWord()) return;
        SquareManager.Instance.ShowRandomWord();
        OnHintWordUsed?.Invoke();
        UpdateHintAmount();
    }

    private void TryToUseHintLetter()
    {
        

        OnHintLetterUsed?.Invoke();
        UpdateHintAmount();
    }
    



    private void ShowAndIncrease()
    {

        _buttonCanvasGroup.alpha = 0;
        _buttonCanvasGroup.interactable = false;
        _buttonCanvasGroup.ignoreParentGroups = true;

        _adIconCanvasGroup.alpha = 0;

        bool flag = false;


        TweenFloat.Create()
        .Origin(0f)
        .Destination(1f)
        .Easing(Ease.Circ)
        .Duration(TweenSettings.Instance.ProgressBarResetTime)
        .OnUpdate(tween =>
        {
            _buttonCanvasGroup.alpha = tween.Value;
            if (!flag && tween.Value >= 0.5f)
            {
                if (_isWordHint)
                {
                    _text.text = "+1";
                }
                else
                {
                    _text.text = "+2";
                }
                SoundManager.Instance.Play(SoundManager.SoundInfoName.letterButtonClicked, 1.3f);
                flag = true;
            }
        })
        .OnEnd(tween =>
        {
            _text.text = (_isWordHint) ? PlayerManager.Instance.GetHintWordAmount().ToString() : PlayerManager.Instance.GetHintLetterAmount().ToString();
            SoundManager.Instance.Play(SoundManager.SoundInfoName.letterButtonClicked);
        })
        .Start();

    }
    
    









}
