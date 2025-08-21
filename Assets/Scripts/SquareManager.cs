using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using FronkonGames.TinyTween;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;



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
    [SerializeField] private Image _levelEndBlinkImage;


    private LevelSO _levelSO;
    public Action OnLevelSOSetupped;
    public static Action<string> OnWrongWordEntered;
    public static Action<string> OnExistingWordEntered;
    public static Action OnCorrectWordEntered;


    private Dictionary<int, Square> _idToSquareDict = new();
    [SerializeField][HideInInspector]
    private List<Square> _squareList = new();
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
    }

    void Start()
    {
        SerializeDictionary();
        ClearSquares();
    }

    private void SerializeDictionary()
    {
        _idToSquareDict = new();
        for (int i = 0; i <= 48; i++)
        {
            _idToSquareDict.Add(i, _squareList[i]);
        }
    }

   

    private void SetLayout(string layout)
    {
        ClearSquares();

        int i = 0;
        foreach (var c in layout)
        {
            if (c != ' ')
            {
                _idToSquareDict[i].SetEmpty(false);
                _idToSquareDict[i].SetLetter(c.ToString());
            }

            i++;
        }
    }


    private void ClearSquares()
    {
        foreach (var square in _idToSquareDict.Values)
        {
            square.SetEmpty(true);
            square.SetGuessed(false);
        }
    }

    public void SetupNewLevelSO(LevelSO levelSO)
    {
        _levelSO = levelSO;


        SetLayout(_levelSO.LayoutString.GetGridString());
        _guessedWordsDictionary.Clear();
        _numOfWords = _levelSO.GetNumOfWords();

        foreach (string word in _levelSO.WordsPositions.Keys)
        {
            _guessedWordsDictionary.Add(word, false);
        }

        SoundManager.Instance.Play(SoundManager.SoundInfoName.levelBegin,0.6f);


        OnLevelSOSetupped?.Invoke();
    }


    public void ShowTheWord(string word)
    {
        word = word.ToUpper();

        if (IsWordAlreadyGuessed(word))
        {
            OnExistingWordEntered?.Invoke(word);
            SoundManager.Instance.Play(SoundManager.SoundInfoName.wordNotGuessed, 1.2f);

            return;
        }

        int[] ids;
        if (_levelSO.WordsPositions.TryGetValue(word, out ids)) // word guessed
        {
            StartCoroutine(SetSquaresAsGuessed(ids));
            CorrectWordBlink();
            _guessedWordsDictionary[word] = true;
            OnCorrectWordEntered?.Invoke();

            _numOfWords--;


            if (_numOfWords == 0) // end of the level
            {
                SoundManager.Instance.Play(SoundManager.SoundInfoName.levelEnd, 0.3f);
                StartCoroutine(InitializeGameEndBehaviourWithDelay());
            }
        }
        else
        {
            OnWrongWordEntered?.Invoke(word);
            SoundManager.Instance.Play(SoundManager.SoundInfoName.wordNotGuessed);

        }
    }

    private void CorrectWordBlink()
    {
        TweenFloat.Create()
        .Origin(0f)
        .Destination(0.05f)
        .Duration(0.35f)
        .Easing(Ease.Quad)
        .Condition(tween => tween.ExecutionCount < 2)
        .Loop(TweenLoop.YoYo)
        .OnUpdate(tween => _levelEndBlinkImage.color = new Color(_levelEndBlinkImage.color.r, _levelEndBlinkImage.color.g, _levelEndBlinkImage.color.b, tween.Value))
        .Start();
        SoundManager.Instance.Play(SoundManager.SoundInfoName.levelEnd, UnityEngine.Random.Range(0.6f, 0.66f));
    }

    IEnumerator InitializeGameEndBehaviourWithDelay()
    {
        yield return new WaitForSeconds(TweenSettings.Instance.GameEndDelay);
        CanvasManager.Instance.GameEndBehaviour();

    }

    IEnumerator SetSquaresAsGuessed(int[] ids)
    {

        foreach (var id in ids)
        {
            yield return new WaitForSeconds(0.1f);

            if (!_idToSquareDict[id].GetIsGuessed())
            {
                _idToSquareDict[id].SetGuessed(true);

                SoundManager.Instance.Play(SoundManager.SoundInfoName.letterButtonClicked);

            }

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


    public bool CanShowRandomWord()
    {
        if (_numOfWords == 0) return false;
        else return true;
    }

    [Button]
    private void FillSquaresList()
    {
        int id = 0;
        _idToSquareDict.Clear();
        foreach (Transform child in transform)
        {
            Square newSquare = child.GetComponent<Square>();
            newSquare.SetID(id);
            _squareList.Add(newSquare);
            Debug.Log("square added: " + id);
            id++;
        }
    }






}