using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class SquareVisuals : MonoBehaviour
{

    [SerializeField] private Image _image;

    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Sprite _letterSprite;




    public void UpdateVisuals(bool isEmpty, string letter, bool isLetterGuessed)
    {
        if (isEmpty)
        {
            _image.sprite = _emptySprite;
            return;
        }

        _image.sprite = _letterSprite;

        if (isLetterGuessed)
        {
            _text.text = letter;
        }
        else
        {
            _text.text = "";
        }

        
    }

}