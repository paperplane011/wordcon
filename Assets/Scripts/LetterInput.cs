using System;
using TMPro;
using UnityEngine;



public class LetterInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _inputText;

    public static Action OnWordEnterEnd;

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
        _inputText.text = _inputText.text + c;
    }

    [ContextMenu("reset")]
    public void Reset()
    {
        _inputText.text = "";
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && _inputText.text != "")
        {
            // check if word is valid then reset letter buttons
            OnWordEnterEnd?.Invoke();
            Reset();

        }

    }
}
