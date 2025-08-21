using System;
using FronkonGames.TinyTween;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SquareVisuals))]
public class Square : MonoBehaviour
{
    [SerializeField] RectTransform _rectTransform;

    
    [SerializeField] public bool _isEmpty = false;

    
    [SerializeField] private string _letter = "";

    
    [SerializeField] private bool _isLetterGuessed = false;

    private SquareVisuals _squareVisuals;
    [SerializeField] private Button _button;

    public int ID { get; private set; }

    public static Action OnSquareClickedAfterHintUsed;



    private void Awake()
    {
        _squareVisuals = GetComponent<SquareVisuals>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(ClickedAfterLetterHintUsed);
        HintButton.OnHintLetterUsed += OnHintLetterUsed;
        OnSquareClickedAfterHintUsed += () => _button.interactable = false;
        ShutterPanel.OnHintCanceled += () => _button.interactable = false;
        
    }

    void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
        HintButton.OnHintLetterUsed -= OnHintLetterUsed;
        OnSquareClickedAfterHintUsed -= () => _button.interactable = false;
        ShutterPanel.OnHintCanceled -= () => _button.interactable = false;
    }



    private void Start()
    {
        _button.interactable = false;
        UpdateVisuals();
    }

    private void OnHintLetterUsed()
    {
        if(!_isLetterGuessed & !_isEmpty) _button.interactable = true;
    }

    private void ClickedAfterLetterHintUsed()
    {
        SetGuessed(true);
        SoundManager.Instance.Play(SoundManager.SoundInfoName.letterButtonClicked);

        OnSquareClickedAfterHintUsed?.Invoke();
    }
    

    public void SetID(int newID)
    {
        ID = newID;
    }

    public void SetEmpty(bool isEmpty)
    {
        _isEmpty = isEmpty;
        UpdateVisuals();
    }

    public void SetLetter(string letter)
    {
        _letter = letter;
        UpdateVisuals();
    }

    public void SetGuessed(bool isGuessed)
    {
        _isLetterGuessed = isGuessed;
        Vector3 _origPos = _rectTransform.anchoredPosition;

        if (isGuessed)
        {
            TweenFloat.Create()
        .Origin(0)
        .Destination(15f)
        .Duration(0.2f)
        .Easing(Ease.Linear)
        .Loop(TweenLoop.YoYo)
        .Condition(tween => tween.ExecutionCount < 2)
        .OnUpdate(tween => _rectTransform.anchoredPosition = new Vector3(_origPos.x, _origPos.y - tween.Value))
        .Start();
        }
        

        UpdateVisuals();
    }


    private void UpdateVisuals()
    {
        _squareVisuals.UpdateVisuals(_isEmpty, _letter, _isLetterGuessed);
    }


    public bool GetIsEmpty()
    {
        return _isEmpty;
    }

    public string GetLetter()
    {
        return _letter;
    }

    public bool GetIsGuessed()
    {
        return _isLetterGuessed;
    }

}

