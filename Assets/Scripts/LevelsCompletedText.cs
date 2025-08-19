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
        CanvasEventBus.OnLevelsEnd += Reset;
    }

    void OnDisable()
    {
        CanvasEventBus.OnResultsLoaded -= ShowNextLevel;
        CanvasEventBus.OnLevelsEnd -= Reset;
    }

    void Start()
    {
        Reset();
    }

    private void Reset()
    {
        _levelsCompleted = PlayerManager.Instance.GetCurrentLevelNum();
        _text.text = _levelsCompleted.ToString() + _postfix;
    }


    private void ShowNextLevel()
    {
        TweenFloat.Create()
        .Origin(0)
        .Destination(1f)
        .Duration(0.8f)
        .Easing(Ease.Linear)
        .OnEnd(tween =>
        {
            _text.text = (++_levelsCompleted).ToString() + _postfix;
            if ((_levelsCompleted - 1) % 5 == 0)
            {
                SoundManager.Instance.Play(SoundManager.SoundInfoName.progressBarStep, 1.2f);
            }
            else
            {
                SoundManager.Instance.Play(SoundManager.SoundInfoName.progressBarStep);
            }

        })
        .Start();


    }


    
}
