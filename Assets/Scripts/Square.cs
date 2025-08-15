using FronkonGames.TinyTween;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(SquareVisuals))]
[ExecuteInEditMode]
public class Square : MonoBehaviour
{
    [SerializeField] RectTransform _rectTransform;

    [OnValueChanged("OnEmptyChanged")]
    [SerializeField] public bool _isEmpty = false;

    [HideIf("_isEmpty")]
    [OnValueChanged("OnLetterChanged")]
    [SerializeField] private string _letter = "";

    [HideIf("_isEmpty")]
    [OnValueChanged("OnIsLetterGuessedChanged")]
    [SerializeField] private bool _isLetterGuessed = false;

    private SquareVisuals _squareVisuals;
    public int ID { get; private set; }

    private Vector3 _pos = new Vector3();

    [HideInInspector]
    public bool CanBeTweened = false;

    



    private void Awake()
    {
        _squareVisuals = GetComponent<SquareVisuals>();
    }

    private void Start()
    {
        
        UpdateVisuals();
    }

   

    void Update()
    {
        if (CanBeTweened) {
            _rectTransform.position = new Vector3(_pos.x, _pos.y + SquareManager.Instance.SquareTweenDelta);
        }
        
    }



    public void SetID(int newID)
    {
        ID = newID;
        _pos = _rectTransform.position;
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

        TweenFloat.Create()
        .Origin(0)
        .Destination(10f)
        .Duration(0.2f)
        .Easing(Ease.Linear)
        .Loop(TweenLoop.YoYo)
        .Condition(tween => tween.ExecutionCount<2)
        .OnUpdate(tween => _rectTransform.position = new Vector3(_rectTransform.position.x, _rectTransform.position.y - tween.Value))
        .Start();

        UpdateVisuals();
    }


    private void UpdateVisuals()
    {
        _squareVisuals.UpdateVisuals(_isEmpty, _letter, _isLetterGuessed);
    }

    private void OnLetterChanged()
    {
        UpdateVisuals();
    }

    private void OnEmptyChanged()
    {
        UpdateVisuals();
    }

    private void OnIsLetterGuessedChanged()
    {
        UpdateVisuals();
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

