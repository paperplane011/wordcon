using System;
using System.Collections.Generic;
using FronkonGames.TinyTween;
using UnityEngine;
using UnityEngine.Windows;



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
    [SerializeField] private float _pitchMultiplierStep = 0.05f;
    private float _pitchMultiplier = 1f;
    private LevelSO _levelSO;
    public Action OnLevelSOSetupped;

    private Dictionary<int, Square> _idToSquareDict;
    private List<GameObject> _spawnedSquaresList = new();
    private int _numOfWords;

    private Dictionary<string, bool> _guessedWordsDictionary = new();

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

    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.C))
        {
            foreach (var word in _levelSO.WordsPositions.Keys)
            {
                ShowTheWord(word);
            }
        }
    }

    void OnEnable()
    {
        LetterInput.OnWordEnterEndWord += ShowTheWord;
        CanvasEventBus.OnGameLoaded += OnGameCanvasLoadedBehaviour;
    }

    void OnDisable()
    {
        LetterInput.OnWordEnterEndWord -= ShowTheWord;
        CanvasEventBus.OnGameLoaded -= OnGameCanvasLoadedBehaviour;
    }

    private void OnGameCanvasLoadedBehaviour()
    {
        if (LevelManager.Instance.TryGetLevelSOByLevelNum(PlayerManager.Instance.GetCurrentLevelNum(), out LevelSO levelSO))
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
        _pitchMultiplier = 1f;

        SetLayout(_levelSO.LayoutString.GetGridString());
        _guessedWordsDictionary.Clear();
        _numOfWords = _levelSO.GetNumOfWords();

        foreach(string word in _levelSO.WordsPositions.Keys)
        {
            _guessedWordsDictionary.Add(word, false);
        }

        SoundManager.Instance.Play(SoundManager.SoundInfoName.levelBegin);


        OnLevelSOSetupped?.Invoke();
    }


    public void ShowTheWord(string word)
    {
        word = word.ToUpper();

        if (IsWordAlreadyGuessed(word))
        {
            Debug.Log("Word " + word + " already guessed!");
            SoundManager.Instance.Play(SoundManager.SoundInfoName.wordNotGuessed, 1.2f);

            return;
        }

        int[] ids;
        if (_levelSO.WordsPositions.TryGetValue(word, out ids))
        {
            SetSquaresAsGuessed(ids);
            _guessedWordsDictionary[word] = true;


            _numOfWords--;

            if (_numOfWords == 0) _pitchMultiplier = 1f;
            SoundManager.Instance.Play(SoundManager.SoundInfoName.wordGuessed, _pitchMultiplier);
            _pitchMultiplier += _pitchMultiplierStep;

            if (_numOfWords == 0)
            {
                TweenFloat.Create()
                .Origin(0f)
                .Destination(1f)
                .Duration(1.5f)
                .Easing(Ease.Linear)
                .OnEnd(tween =>
                {
                    CanvasManager.Instance.GameEndBehaviour();
                    SoundManager.Instance.Play(SoundManager.SoundInfoName.levelEnd);
                })
                .Start();
            }
        }
        else
        {
            Debug.Log("No such word: " + word);
            SoundManager.Instance.Play(SoundManager.SoundInfoName.wordNotGuessed);
            _pitchMultiplier = 1f;
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
        if (_guessedWordsDictionary.ContainsKey(word) && _guessedWordsDictionary[word] == true)
        {
            return true;
        }

        return false;
    }


    public void ShowRandomWord()
    {
        foreach (string word in _guessedWordsDictionary.Keys)
        {
            if (_guessedWordsDictionary[word] == false)
            {
                ShowTheWord(word);
                return;
            }
        }
    }




    


}