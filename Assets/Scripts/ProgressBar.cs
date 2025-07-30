using FronkonGames.TinyTween;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform _selectionTransform;
    [SerializeField] private RectTransform _pos1;
    [SerializeField] private RectTransform _pos2;
    [SerializeField] private RectTransform _pos3;
    [SerializeField] private RectTransform _pos4;
    [SerializeField] private RectTransform _pos5;

    private int _currentPosNum = 1;

    void OnEnable()
    {
        CanvasEventBus.OnResultsLoaded += GoToNextPos;
    }

    void OnDisable()
    {
        CanvasEventBus.OnResultsLoaded += GoToNextPos;
    }



    private void GoToNextPos()
    {
        Vector2 newPos = new();

        _currentPosNum++;

        if (_currentPosNum == 2) newPos = _pos2.position;
        else if (_currentPosNum == 3) newPos = _pos3.position;
        else if (_currentPosNum == 4) newPos = _pos4.position;
        else if (_currentPosNum == 5) newPos = _pos5.position;
        else if (_currentPosNum == 1) newPos = _pos1.position;


        _selectionTransform.TweenMove(newPos, 1.5f, Ease.Quint);

        

    }

    



}
