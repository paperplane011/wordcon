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

    private Button _button;
    private CanvasGroup _buttonCanvasGroup;
    

    public static Action OnHintUsed;

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
        CanvasEventBus.OnGameLoaded += () => _buttonCanvasGroup.interactable = true; 
    }

    void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
        ProgressBar.OnProgressBarReset -= ShowAndIncrease;
        CanvasEventBus.OnGameLoaded -= () => _buttonCanvasGroup.interactable = true; 
    }

    void Start()
    {
        UpdateHintAmount();
    }


    private void UpdateHintAmount()
    {
        int newHintAmount = PlayerManager.Instance.GetHintAmount();


        if (newHintAmount <= 0)
        {
            _doesHaveHints = false;
            _adIconCanvasGroup.alpha = 1f;
            _text.text = "+3";
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
            TryToUseHint();
        }
        else
        {
            AddHintsForAd();
        }

    }

    private void AddHintsForAd()
    {
        YG2.RewardedAdvShow("0");
        
        YG2.saves.hintAmount += 3;
        YG2.SaveProgress();

        _buttonCanvasGroup.interactable = false;
        UpdateHintAmount();
    }

    private void TryToUseHint()
    {
        
        if (!SquareManager.Instance.CanShowRandomWord()) return;
        SquareManager.Instance.ShowRandomWord();
        OnHintUsed?.Invoke();
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
                _text.text += "+2";
                SoundManager.Instance.Play(SoundManager.SoundInfoName.letterButtonClicked, 1.3f);
                flag = true;
            }
        })
        .OnEnd(tween =>
        {
            _text.text = PlayerManager.Instance.GetHintAmount().ToString();
            SoundManager.Instance.Play(SoundManager.SoundInfoName.letterButtonClicked);
        })
        .Start();
        
    }
    
    









}
