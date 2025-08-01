using System;
using TMPro;
using UnityEngine;



public class LetterInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _inputTextComp;
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
    }

    void OnDisable()
    {
        LetterButton.OnLetterButtonPressed -= AddChar;
    }

    public void AddChar(char c)
    {
        _inputText = _inputText + c;
        _inputTextComp.text = _inputText;

        SoundManager.Instance.Play(SoundManager.SoundInfoName.letterButtonClicked, _pitchMultiplier);
        _pitchMultiplier += _pitchMultiplierStep;
    }


    public void Reset()
    {
        _inputText = "";
        _inputTextComp.text = _inputText;
        _pitchMultiplier = 1f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && _inputText != "")
        {
            // check if word is valid then reset letter buttons
            

            OnWordEnterEndWord?.Invoke(_inputText);
            OnWordEnterEnd?.Invoke();
            Reset();

        }

    }
}
