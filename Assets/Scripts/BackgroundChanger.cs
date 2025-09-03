using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using FronkonGames.TinyTween;
using YG;



#if UNITY_EDITOR
using UnityEditor;
#endif



public class BackgroundChanger : MonoBehaviour
{

    [SerializeField] private Image _mainImage;
    [SerializeField] private Image _backImage;
    [SerializeField] private CanvasGroup _mainImageCanvasGroup;
    [SerializeField] private CanvasGroup _backImageCanvasGroup;
    private RectTransform _mainImageRectTransform;
    private RectTransform _backImageRectTransform;



    [SerializeField]
    [ReadOnly]
    private List<Sprite> _backgroundsList;

    private int _currentBackgroundNum = 0;

    void Awake()
    {
        _mainImageRectTransform = _mainImage.GetComponent<RectTransform>();
        _backImageRectTransform = _backImage.GetComponent<RectTransform>();
    }


    void Start()
    {
        _currentBackgroundNum = PlayerManager.Instance.GetCurrentLevelNum()/5;
       
        SetupAnchors();
        if (PlayerManager.Instance.GetCurrentLevelNum() % 5 == 0) _currentBackgroundNum -= 1;

        _mainImage.sprite = _backgroundsList[_currentBackgroundNum];
        _mainImageCanvasGroup.alpha = 1;
        
        
    }

    private void SetupAnchors()
    {
        
        if (YG2.envir.isMobile) // stretch
        {
            // 1. Устанавливаем якоря в противоположные углы родителя
            // anchorMin (0,0) - нижний левый угол родителя
            // anchorMax (1,1) - верхний правый угол родителя
            _mainImageRectTransform.anchorMin = Vector2.zero;
            _mainImageRectTransform.anchorMax = Vector2.one;

            _backImageRectTransform.anchorMin = Vector2.zero;
            _backImageRectTransform.anchorMax = Vector2.one;


            // 2. Обнуляем ВСЕ отступы от краев
            // offsetMin - отступы слева и снизу (Left, Bottom)
            // offsetMax - отступы справа и сверху (-Right, -Top)
            _mainImageRectTransform.offsetMin = Vector2.zero;
            _mainImageRectTransform.offsetMax = Vector2.zero;

            _backImageRectTransform.offsetMin = Vector2.zero;
            _backImageRectTransform.offsetMax = Vector2.zero;
        }
    }

    

    private void OnEnable()
    {
        ProgressBar.OnProgressBarReset += SetNextBackground;
        CanvasEventBus.OnLevelsEnd += Reset;
    }

    private void OnDisable()
    {
        ProgressBar.OnProgressBarReset -= SetNextBackground;
        CanvasEventBus.OnLevelsEnd -= Reset;
    }

    private void Reset()
    {
        _currentBackgroundNum = 0;
        
        _mainImage.sprite = _backgroundsList[_currentBackgroundNum];
        _mainImageCanvasGroup.alpha = 1;
    }



    public void SetNextBackground()
    {
        _currentBackgroundNum++;
        if (_currentBackgroundNum >= _backgroundsList.Count) return;

        
        _backImage.sprite = _backgroundsList[_currentBackgroundNum];



        TweenFloat.Create()
        .Origin(1f)
        .Destination(0f)
        .Duration(TweenSettings.Instance.ProgressBarResetTime)
        .Easing(Ease.Linear)
        .OnUpdate(tween => _mainImageCanvasGroup.alpha = tween.Value)
        .OnEnd(tween =>
        {
            _mainImage.sprite = _backgroundsList[_currentBackgroundNum];

        })
        .Start();
    }

    
    [Button("Fill Backgrounds List")]
    private void GetAllBackgrounds()
    {
        _backgroundsList = new();

#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets("l:Terrain");

        foreach (string guid in guids)
        {

            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite backgroundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (backgroundSprite != null)
            {
                _backgroundsList.Add(backgroundSprite);
                Debug.Log("background added: " + backgroundSprite.name);
            }
        }
#endif
    }




}