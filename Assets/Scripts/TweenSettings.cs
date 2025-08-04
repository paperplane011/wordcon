using System;
using UnityEngine;


public class TweenSettings : MonoBehaviour
{
    private static TweenSettings _instance;

    public static TweenSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<TweenSettings>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(TweenSettings).Name);
                    _instance = singletonObject.AddComponent<TweenSettings>();
                }
            }
            return _instance;
        }
    }



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



    public float ProgressBarGoToNextPosTime = 1.5f;
    public float ProgressBarResetTime = 1f;
    public float NextLevelButtonFadeInTime = 1f;
    public float DefaultCanvasFadeTime = 0.6f;
    public float ResultsCanvasFadeTime = 1.6f;
    public float GuessedSquaresFadeInTime = 1f;


}