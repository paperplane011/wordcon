using System;
using System.Collections;
using System.Collections.Generic;
using FronkonGames.TinyTween;
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
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private Image _levelEndBlinkImage;
    private RectTransform _gridLayoutGroupRectTransform;

    private LevelSO _levelSO;
    public Action OnLevelSOSetupped;

    private Dictionary<int, Square> _idToSquareDict;
    private int _numOfWords;

    private Dictionary<string, bool> _guessedWordsDictionary = new();

    [HideInInspector]
    public float SquareTweenDelta = 0f;

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

        _gridLayoutGroupRectTransform = _gridLayoutGroup.GetComponent<RectTransform>();
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
        SpawnSquares();
    }

    private void SpawnSquares()
    {
        _idToSquareDict = new();

        for (int i = 0; i <= 48; i++)
        {
            GameObject newSquareGO = Instantiate(_squarePrefab, transform);
            Square newSquare = newSquareGO.GetComponent<Square>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_gridLayoutGroupRectTransform);
            newSquare.SetID(i);
            _idToSquareDict.Add(i, newSquare);

        }

        _gridLayoutGroup.enabled = false;

        TweenFloat.Create()
        .Origin(-TweenSettings.Instance.SquareFloatPosDelta)
        .Destination(TweenSettings.Instance.SquareFloatPosDelta)
        .Duration(TweenSettings.Instance.SquareFloatSpeed)
        .Easing(Ease.Sine)
        .OnUpdate(tween => SquareTweenDelta = tween.Value)
        .Loop(TweenLoop.YoYo)
        .Start();

        foreach (Square square in _idToSquareDict.Values)
        {
            square.CanBeTweened = true;
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
        if (_levelSO.WordsPositions.TryGetValue(word, out ids)) // word guessed
        {
            StartCoroutine(SetSquaresAsGuessed(ids));
            _guessedWordsDictionary[word] = true;

            _numOfWords--;


            if (_numOfWords == 0)
            {
                LevelCompleteBlink();
                StartCoroutine(InitializeGameEndBehaviourWithDelay());
            }
        }
        else
        {
            Debug.Log("No such word: " + word);
            SoundManager.Instance.Play(SoundManager.SoundInfoName.wordNotGuessed);

        }
    }

    private void LevelCompleteBlink()
    {
        TweenFloat.Create()
        .Origin(0f)
        .Destination(0.06f)
        .Duration(0.33f)
        .Easing(Ease.Linear)
        .Condition(tween => tween.ExecutionCount < 2)
        .Loop(TweenLoop.YoYo)
        .OnUpdate(tween => _levelEndBlinkImage.color = new Color(_levelEndBlinkImage.color.r, _levelEndBlinkImage.color.g, _levelEndBlinkImage.color.b, tween.Value))
        .Start();
        SoundManager.Instance.Play(SoundManager.SoundInfoName.levelEnd);
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




    


}