using System;
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
    private LevelSO _levelSO;
    public Action OnLevelSOSetupped;

    private Dictionary<int, Square> _idToSquareDict;
    private List<GameObject> _spawnedSquaresList = new();
    private int _numOfWords;

    private List<string> _guessedWords = new();

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


    void OnEnable()
    {
        LetterInput.OnWordEnterEndWord += CheckIfWordIsValid;
        CanvasEventBus.OnGameLoaded += OnGameCanvasLoadedBehaviour;
    }

    void OnDisable()
    {
        LetterInput.OnWordEnterEndWord -= CheckIfWordIsValid;
        CanvasEventBus.OnGameLoaded -= OnGameCanvasLoadedBehaviour;
    }

    private void OnGameCanvasLoadedBehaviour()
    {
        if (LevelManager.Instance.TryGetLevelSOByLevelNum(PlayerManager.Instance.GetLastLevelNum(), out LevelSO levelSO))
        {
            SetupNewLevelSO(levelSO);
        }
        else
        {
            CanvasManager.Instance.OnLevelsEndBehaviour();
        }

    }

    private void SetLayout(string layout)
    {
        int id = 0;
        _idToSquareDict = new();
        _idToSquareDict.Clear();

        DeleteExistingSquares();

        foreach (var c in layout)
        {
            GameObject newSquareGO = Instantiate(_squarePrefab, transform);
            _spawnedSquaresList.Add(newSquareGO);

            Square newSquare = newSquareGO.GetComponent<Square>();

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

    private void DeleteExistingSquares()
    {
        if (_spawnedSquaresList.Count == 0) return;
        foreach (var square in _spawnedSquaresList)
        {
            Destroy(square);
        }
        _spawnedSquaresList.Clear();
    }

    public void SetupNewLevelSO(LevelSO levelSO)
    {
        _levelSO = levelSO;

        SetLayout(_levelSO.LayoutString.GetGridString());
        _guessedWords.Clear();
        _numOfWords = _levelSO.GetNumOfWords();

        SoundManager.Instance.Play(SoundManager.SoundInfoName.levelBegin);
        

        OnLevelSOSetupped?.Invoke();
    }


    public void CheckIfWordIsValid(string word)
    {
        word = word.ToUpper();

        if (IsWordAlreadyGuessed(word))
        {
            Debug.Log("Word " + word + " already guessed!");
            SoundManager.Instance.Play(SoundManager.SoundInfoName.wordNotGuessed);
            return;
        } 

        int[] ids;
        if (_levelSO.WordsPositions.TryGetValue(word, out ids))
        {
            SetSquaresAsGuessed(ids);
            _guessedWords.Add(word);

            SoundManager.Instance.Play(SoundManager.SoundInfoName.wordGuessed);
            _numOfWords--;

            if (_numOfWords == 0)
            {
                CanvasManager.Instance.GameEndBehaviour();
                SoundManager.Instance.Play(SoundManager.SoundInfoName.levelEnd);
            }
        }
        else
        {
            Debug.Log("No such word: " + word);
            SoundManager.Instance.Play(SoundManager.SoundInfoName.wordNotGuessed);
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


    public bool IsWordAlreadyGuessed(string word)
    {
        return _guessedWords.Contains(word);
    }




    


}