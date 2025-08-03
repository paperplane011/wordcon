using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
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

    [SerializeField]
    [ReadOnly]
    private List<Sprite> _backgroundsList;

    private int _currentBackgroundNum = 0;




    void Start()
    {
        _mainImage.sprite = _backgroundsList[_currentBackgroundNum];
        _mainImageCanvasGroup.alpha = 1;
        
        
    }

    private void OnEnable()
    {
        ProgressBar.OnProgressBarReset += SetNextBackground;
    }

    private void OnDisable()
    {
        ProgressBar.OnProgressBarReset -= SetNextBackground;
    }



    public void SetNextBackground()
    {
        _currentBackgroundNum++;
        _backImage.sprite = _backgroundsList[_currentBackgroundNum];
        

        // Fade Out
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