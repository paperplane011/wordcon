using System;
using TMPro;
using UnityEngine;



public class LetterInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _inputTextComp;
    [SerializeField] private RectTransform _panelRectTransform;

    private string _inputText;

    

    public static Action<string> OnWordEnterEndWord;
    public static Action OnWordEnterEnd;

    [SerializeField] private float _pitchMultiplierStep=0.1f;
    private float _pitchMultiplier = 1f;
    

    private void Start()
    {
        Reset();
    }

    void OnEnable()
    {
        LetterButton.OnLetterButtonPressed += AddChar;
        SquareManager.OnWrongWordEntered += ShowWrongWordMessage;
        SquareManager.OnExistingWordEntered += ShowExistingWordMessage;
        SquareManager.OnCorrectWordEntered += () => Reset();

        HintButton.OnHintLetterUsed += ShowLetterHintMessage;
        Square.OnSquareClickedAfterHintUsed += () => Reset();
    }

    void OnDisable()
    {
        LetterButton.OnLetterButtonPressed -= AddChar;
        SquareManager.OnWrongWordEntered -= ShowWrongWordMessage;
        SquareManager.OnExistingWordEntered -= ShowExistingWordMessage;
        SquareManager.OnCorrectWordEntered -= () => Reset();

        HintButton.OnHintLetterUsed -= ShowLetterHintMessage;
        Square.OnSquareClickedAfterHintUsed -= () => Reset();
    }

    public void AddChar(char c)
    {
        _inputText = _inputText + c;

        _inputTextComp.text = _inputText;

        SoundManager.Instance.Play(SoundManager.SoundInfoName.letterButtonClicked, _pitchMultiplier);
        _pitchMultiplier += _pitchMultiplierStep;

        if (_inputText.Length == 1)
        {
            _panelRectTransform.sizeDelta = new Vector2(110f, _panelRectTransform.sizeDelta.y);
        }
        else
        {
            _panelRectTransform.sizeDelta = new Vector2(110f + _inputTextComp.textBounds.size.x, _panelRectTransform.sizeDelta.y);
        }
    }


    public void Reset()
    {
        _inputText = "";
        _inputTextComp.text = _inputText;
        _pitchMultiplier = 1f;
        
        _panelRectTransform.sizeDelta = new Vector2(0f, _panelRectTransform.sizeDelta.y);
    }

    private void ShowWrongWordMessage(string word)
    {
        if (word.Length < 3)
        {
            Reset();
            return;
        }
        _inputText = "";
        _pitchMultiplier = 1f;
        
        _inputTextComp.text = $"СЛОВА «{word}» НЕТ В КРОССВОРДЕ";
        _inputTextComp.ForceMeshUpdate();
        _panelRectTransform.sizeDelta = new Vector2(110f + _inputTextComp.textBounds.size.x, _panelRectTransform.sizeDelta.y);

    }

    private void ShowExistingWordMessage(string word)
    {
        _inputText = "";
        _pitchMultiplier = 1f;
         _inputTextComp.text = $"СЛОВО «{word}» УЖЕ ОТКРЫТО";
         _inputTextComp.ForceMeshUpdate();
        _panelRectTransform.sizeDelta = new Vector2(110f + _inputTextComp.textBounds.size.x, _panelRectTransform.sizeDelta.y);
    }

    private void ShowLetterHintMessage()
    {
        _inputText = "";
        _pitchMultiplier = 1f;

        _inputTextComp.text = $"НАЖМИТЕ НА БУКВУ КОТОРУЮ ХОТИТЕ ОТКРЫТЬ";
        _inputTextComp.ForceMeshUpdate();
        _panelRectTransform.sizeDelta = new Vector2(110f + _inputTextComp.textBounds.size.x, _panelRectTransform.sizeDelta.y);
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && _inputText != "")
        {
            // check if word is valid then reset letter buttons
            OnWordEnterEndWord?.Invoke(_inputText);
            OnWordEnterEnd?.Invoke();
        }

    }
}
