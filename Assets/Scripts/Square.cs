using FronkonGames.TinyTween;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(SquareVisuals))]
[ExecuteInEditMode]
public class Square : MonoBehaviour
{
    [SerializeField] RectTransform _rectTransform;

    
    [SerializeField] public bool _isEmpty = false;

    
    [SerializeField] private string _letter = "";

    
    [SerializeField] private bool _isLetterGuessed = false;

    private SquareVisuals _squareVisuals;
    public int ID { get; private set; }

    

    private void Awake()
    {
        _squareVisuals = GetComponent<SquareVisuals>();
    }

    private void Start()
    {
        
        UpdateVisuals();
    }

    public void SetID(int newID)
    {
        ID = newID;
    }

    public void SetEmpty(bool isEmpty)
    {
        _isEmpty = isEmpty;
        UpdateVisuals();
    }

    public void SetLetter(string letter)
    {
        _letter = letter;
        UpdateVisuals();
    }

    public void SetGuessed(bool isGuessed)
    {
        _isLetterGuessed = isGuessed;
        Vector3 _origPos = _rectTransform.anchoredPosition;

        if (isGuessed)
        {
            TweenFloat.Create()
        .Origin(0)
        .Destination(10f)
        .Duration(0.2f)
        .Easing(Ease.Linear)
        .Loop(TweenLoop.YoYo)
        .Condition(tween => tween.ExecutionCount < 2)
        .OnUpdate(tween => _rectTransform.anchoredPosition = new Vector3(_origPos.x, _origPos.y - tween.Value))
        .Start();
        }
        

        UpdateVisuals();
    }


    private void UpdateVisuals()
    {
        _squareVisuals.UpdateVisuals(_isEmpty, _letter, _isLetterGuessed);
    }


    public bool GetIsEmpty()
    {
        return _isEmpty;
    }

    public string GetLetter()
    {
        return _letter;
    }

    public bool GetIsGuessed()
    {
        return _isLetterGuessed;
    }

}

