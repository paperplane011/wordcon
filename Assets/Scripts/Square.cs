using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(SquareVisuals))]
public class Square : MonoBehaviour
{

    [SerializeField] private bool _isEmpty;

    [HideIf("_isEmpty")]
    [SerializeField] private string _letter = "";

    private SquareVisuals _squareVisuals;
    public int ID { get; private set; }


    private void Awake()
    {
        _squareVisuals = GetComponent<SquareVisuals>();
    }

    private void Start()
    {
        _squareVisuals.SetSprite(_isEmpty, _letter);
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
        _squareVisuals.SetSprite(_isEmpty, _letter);
    }




}

