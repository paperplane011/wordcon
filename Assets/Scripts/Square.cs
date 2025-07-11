using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(SquareVisuals))]
[ExecuteInEditMode]
public class Square : MonoBehaviour
{

    [OnValueChanged("OnEmptyChanged")]
    [SerializeField] private bool _isEmpty = false;

    [HideIf("_isEmpty")]
    [OnValueChanged("OnLetterChanged")]
    [SerializeField] private string _letter = "";

    [HideIf("_isEmpty")]
    [OnValueChanged("OnIsLetterGuessedChanged")]
    [SerializeField] private bool _isLetterGuessed;

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


    private void UpdateVisuals()
    {
        _squareVisuals.SetSprite(_isEmpty, _letter, _isLetterGuessed);
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


}

