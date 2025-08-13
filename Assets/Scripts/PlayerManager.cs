using System;
using UnityEngine;
using YG;


namespace YG
{
    public partial class SavesYG
    {
        public int currentLevel;
        public int hintAmount;
    }
}


public class PlayerManager : MonoBehaviour
{

    [SerializeField]


    private static PlayerManager _instance;

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<PlayerManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(PlayerManager).Name);
                    _instance = singletonObject.AddComponent<PlayerManager>();
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



    void OnEnable()
    {
        CanvasEventBus.OnGameEnd += GameEndBehaviour;
        HintButton.OnHintWithoutAdsUsed += OnHintUsed;
        YG2.onDefaultSaves += EmptySavesBehaviour;
    }

    void OnDisable()
    {
        CanvasEventBus.OnGameEnd -= GameEndBehaviour;
        HintButton.OnHintWithoutAdsUsed -= OnHintUsed;
        YG2.onDefaultSaves -= EmptySavesBehaviour;
    }

    private void OnHintUsed()
    {
        YG2.saves.hintAmount--;
    }


    public int GetCurrentLevelNum()
    {
        return YG2.saves.currentLevel;
    }

    private void GameEndBehaviour()
    {
        YG2.saves.currentLevel++;

        if ((YG2.saves.currentLevel - 1) % 5 == 0)
        {
            YG2.saves.hintAmount+=3;
        }

        YG2.SaveProgress();
    }

    private void EmptySavesBehaviour()
    {
        YG2.saves.currentLevel = 1;
        YG2.saves.hintAmount = 2;
        Debug.Log("first game session!");
    }


    public int GetHintAmount()
    {
        return YG2.saves.hintAmount;
    }



}