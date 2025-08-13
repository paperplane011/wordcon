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

    Vector3 _pos = new Vector3();

    



    private void Awake()
    {
        _squareVisuals = GetComponent<SquareVisuals>();
    }

    private void Start()
    {
        
        UpdateVisuals();
    }

    public void AddTween()
    {
        _rectTransform.position = _pos;

        Vector3 originPos = new Vector3(_rectTransform.position.x, _rectTransform.position.y + TweenSettings.Instance.SquareFloatPosDelta, 0);
        Vector3 destPos = new Vector3(_rectTransform.position.x, _rectTransform.position.y - TweenSettings.Instance.SquareFloatPosDelta, 0);

        _rectTransform.TweenMove(originPos, destPos, TweenSettings.Instance.SquareFloatSpeed, Ease.Sine).Loop(TweenLoop.YoYo);
        
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

