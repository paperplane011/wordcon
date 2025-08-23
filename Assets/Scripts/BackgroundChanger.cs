using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using FronkonGames.TinyTween;


#if UNITY_EDITOR
using UnityEditor;
#endif



public class BackgroundChanger : MonoBehaviour
{

    [SerializeField] private Image _mainImage;
    [SerializeField] private Image _backImage;
    [SerializeField] private CanvasGroup _mainImageCanvasGroup;
    [SerializeField] private CanvasGroup _backImageCanvasGroup;

    [SerializeField] private Image _mainBG16x9;
    [SerializeField] private Image _bordersBG16x9;

    [SerializeField] private Color[] _colorArrayFor16x9BG;

    [SerializeField]
    [ReadOnly]
    private List<Sprite> _backgroundsList;

    private int _currentBackgroundNum = 0;




    void Start()
    {
        _currentBackgroundNum = PlayerManager.Instance.GetCurrentLevelNum()/5;
        SetBG16x9ForBGNum(_currentBackgroundNum);
        if (PlayerManager.Instance.GetCurrentLevelNum() % 5 == 0) _currentBackgroundNum -= 1;

        _mainImage.sprite = _backgroundsList[_currentBackgroundNum];
        _mainImageCanvasGroup.alpha = 1;
        
        
    }

    private void SetBG16x9ForBGNum(int bgNum)
    {
        Color mainColor = _colorArrayFor16x9BG[bgNum];

        TweenColor.Create()
        .Origin(_mainBG16x9.color)
        .Destination(mainColor)
        .Duration(TweenSettings.Instance.ProgressBarResetTime)
        .Easing(Ease.Linear)
        .OnUpdate(tween => _mainBG16x9.color = tween.Value)
        .Start();

        
        Color.RGBToHSV(mainColor, out float h, out float s, out float v);
        Color borderColor = Color.HSVToRGB(h, s, v+0.1f);

        TweenColor.Create()
        .Origin(_bordersBG16x9.color)
        .Destination(borderColor)
        .Duration(TweenSettings.Instance.ProgressBarResetTime)
        .Easing(Ease.Linear)
        .OnUpdate(tween => _bordersBG16x9.color = tween.Value)
        .Start();

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
        SetBG16x9ForBGNum(_currentBackgroundNum);
        _mainImage.sprite = _backgroundsList[_currentBackgroundNum];
        _mainImageCanvasGroup.alpha = 1;
    }



    public void SetNextBackground()
    {
        _currentBackgroundNum++;
        if (_currentBackgroundNum >= _backgroundsList.Count) return;

        SetBG16x9ForBGNum(_currentBackgroundNum);
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