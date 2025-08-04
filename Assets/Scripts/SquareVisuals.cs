using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FronkonGames.TinyTween;


public class SquareVisuals : MonoBehaviour
{

    [SerializeField] private Image _image;

    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private Color _emptySquareColor;
    [SerializeField] private Color _letterSquareNotGuessedColor;
    private Color _letterSquareGuessedColor = Color.white;
    

    public void UpdateVisuals(bool isEmpty, string letter, bool isLetterGuessed)
    {
        if (isEmpty)
        {
            _image.color = new Color(0f, 0f, 0f, 0f);
            _text.text = "";
            return;
        }

        if (isLetterGuessed)
        {
            _text.text = letter;
            

            TweenColor.Create()
            .Origin(_image.color)
            .Destination(_letterSquareGuessedColor)
            .Easing(Ease.Quad)
            .Owner(this)
            .Duration(TweenSettings.Instance.GuessedSquaresFadeInTime)
            .OnUpdate(tween => _image.color = tween.Value)
            .Start();
        }
        else
        {
            _text.text = "";

            TweenColor.Create()
            .Origin(_image.color)
            .Destination(_letterSquareNotGuessedColor)
            .Easing(Ease.Sine)
            .Owner(this)
            .Duration(1f)
            .OnUpdate(tween => _image.color = tween.Value)
            .Start();
        }


    }

}