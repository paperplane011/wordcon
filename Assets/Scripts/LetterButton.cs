using System;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(HoverChecker))]
public class LetterButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _letterText;
    private HoverChecker _hoverChecker;

    public static Action<char> OnLetterButtonPressed;

    public int ID { get; private set; } = -1;
    public char ButtonChar { get; private set; } = 'A';


    private bool _isButtonUsed = false;

    private void Awake()
    {
        _hoverChecker = GetComponent<HoverChecker>();
    }

    void OnEnable()
    {
        LetterInput.OnWordEnterEnd += ResetButton;
    }

    void OnDisable()
    {
        LetterInput.OnWordEnterEnd -= ResetButton;
    }

    private void ResetButton()
    {
        _isButtonUsed = false;
    }

    public void SetButtonChar(char newChar)
    {
        ButtonChar = newChar;
        _letterText.text = newChar.ToString();
    }

    public void SetID(int newID)
    {
        ID = newID;
    }

    void Update()
    {
        if (!_isButtonUsed && _hoverChecker.IsCursorHovered && Input.GetMouseButton(0))
        {
            OnLetterButtonPressed?.Invoke(ButtonChar);
            _isButtonUsed = true;
        }



    }





}
