using FronkonGames.TinyTween;
using TMPro;
using UnityEngine;

public class LevelsCompletedText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _postfix;

    private int _levelsCompleted = 1;

    void OnEnable()
    {
        CanvasEventBus.OnResultsLoaded += ShowNextLevel;
    }

    void OnDisable()
    {
        CanvasEventBus.OnResultsLoaded += ShowNextLevel;
    }

    void Start()
    {
        _text.text = _levelsCompleted.ToString() + _postfix;
    }


    private void ShowNextLevel()
    {
        TweenFloat.Create()
        .Origin(0)
        .Destination(1f)
        .Duration(0.8f)
        .Easing(Ease.Linear)
        .OnEnd(tween => { _text.text = (++_levelsCompleted).ToString() + _postfix; SoundManager.Instance.Play(SoundManager.SoundInfoName.progressBarStep); })
        .Start();


    }
}
