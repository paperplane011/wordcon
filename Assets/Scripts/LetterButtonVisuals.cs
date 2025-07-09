using UnityEngine;
using UnityEngine.UI;

public class LetterButtonVisuals : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _clickedButtonSprite;
    [SerializeField] private Sprite _notClickedButtonSprite;


    void Start()
    {
        ButtonResetBehaviour();
    }

    public void ButtonClickedBehaviour()
    {
        _image.sprite = _clickedButtonSprite;
    }


    public void ButtonResetBehaviour()
    {
        _image.sprite = _notClickedButtonSprite;
    }


}
