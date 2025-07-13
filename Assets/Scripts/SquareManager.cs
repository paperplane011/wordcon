using System.Collections.Generic;
using UnityEngine;



public class SquareManager : MonoBehaviour
{

    private static SquareManager _instance;

    public static SquareManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SquareManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(SquareManager).Name);
                    _instance = singletonObject.AddComponent<SquareManager>();
                }
            }
            return _instance;
        }
    }

    [SerializeField] private GameObject _squarePrefab;
    [SerializeField] private LevelSO _levelSO;

    private Dictionary<int, Square> _idToSquareDict = new();

    private void Awake()
    {
        // Ensure only one instance exists
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        SetLayout(_levelSO.LayoutString.GetGridString());

    }

    void OnEnable()
    {
        LetterInput.OnWordEnterEndWord += CheckIfWordIsValid;
    }

    void OnDisable()
    {
        LetterInput.OnWordEnterEndWord -= CheckIfWordIsValid;
    }

    private void SetLayout(string layout)
    {
        int id = 0;
        foreach (var c in layout)
        {
            var newSquare = Instantiate(_squarePrefab, transform).GetComponent<Square>();
            newSquare.SetID(id);
            _idToSquareDict.Add(id, newSquare);
            id++;

            if (c == ' ')
            {
                newSquare.SetEmpty(true);
            }
            else
            {
                newSquare.SetEmpty(false);
                newSquare.SetLetter(c.ToString());
            }
        }
    }


    public void CheckIfWordIsValid(string word)
    {
        word = word.ToUpper();

        int[] ids;
        if (_levelSO.WordsPositions.TryGetValue(word, out ids))
        {
            SetSquaresAsGuessed(ids);
        }
        else
        {
            Debug.Log("No such word: " + word);
        }
    }


    private void SetSquaresAsGuessed(int[] ids)
    {
        foreach (var id in ids)
        {
            _idToSquareDict[id].SetGuessed(true);
        }
    }


    public string GetLettersForLetterCircle()
    {
        return _levelSO.LevelLetters;
    }




    


}