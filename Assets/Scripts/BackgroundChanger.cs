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

    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _imageCanvasGroup;

    [SerializeField]
    [ReadOnly]
    private List<Sprite> _backgroundsList;

    private int _currentBackgroundNum = 0;




    void Start()
    {
        _image.sprite = _backgroundsList[_currentBackgroundNum];
        _imageCanvasGroup.alpha = 1;
    }

   



    public void SetNextBackground()
    {
        _currentBackgroundNum++;

        // Fade Out
        TweenFloat.Create()
        .Origin(1f)
        .Destination(0f)
        .Duration(TweenSettings.Instance.ProgressBarResetTime)
        .Easing(Ease.Sine)
        .OnUpdate(tween => UpdateBackground(tween.Value))
        .Start();
    }

    private void UpdateBackground(float tweenValue)
    {
        if (tweenValue <= 0.5f)
        {
            _image.sprite = _backgroundsList[_currentBackgroundNum];
            _imageCanvasGroup.alpha = 1f - tweenValue;
        }
        else
        {
            _imageCanvasGroup.alpha = tweenValue;
        }
        
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