using System;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(Button), typeof(Image))]
public class ShutterPanel : MonoBehaviour
{
    private Button _button;
    private Image _image;

    public static Action OnHintCanceled;

    void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }

    void Start()
    {
        _image.enabled = false;
    }

    void OnEnable()
    {
        _button.onClick.AddListener(Clicked);
        HintButton.OnHintLetterUsed += () => _image.enabled = true;
        Square.OnSquareClickedAfterHintUsed += () => _image.enabled = false;
    }

    void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
        HintButton.OnHintLetterUsed -= () => _image.enabled = true;
        Square.OnSquareClickedAfterHintUsed -= () => _image.enabled = false;
    }


    private void Clicked()
    {
        _image.enabled = false;
        OnHintCanceled?.Invoke();
    }

    


}