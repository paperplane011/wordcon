using System;
using UnityEngine;
using YG;


namespace YG
{
    public partial class SavesYG
    {
        public int currentLevel;
        public int hintAmountWord;
        public int hintAmountLetter;

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
        HintButton.OnHintWordUsed += OnHintWordUsed;
        HintButton.OnHintLetterUsed += OnHintLetterUsed;
        YG2.onDefaultSaves += EmptySavesBehaviour;
        CanvasEventBus.OnLevelsEnd += OnLevelEndBehavior;


    }

    void OnDisable()
    {
        CanvasEventBus.OnGameEnd -= GameEndBehaviour;
        HintButton.OnHintWordUsed -= OnHintWordUsed;
        HintButton.OnHintLetterUsed -= OnHintLetterUsed;
        YG2.onDefaultSaves -= EmptySavesBehaviour;
        CanvasEventBus.OnLevelsEnd -= OnLevelEndBehavior;

    }

    private void OnHintWordUsed()
    {
        YG2.saves.hintAmountWord--;
    }

    private void OnHintLetterUsed()
    {
        YG2.saves.hintAmountLetter--;
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
            YG2.saves.hintAmountWord += 1;
            YG2.saves.hintAmountLetter += 2;
        }

        YG2.SaveProgress();
    }

    private void EmptySavesBehaviour()
    {
        YG2.saves.currentLevel = 1;
        YG2.saves.hintAmountWord = 2;
        YG2.saves.hintAmountLetter = 3;
        Debug.Log("first game session!");
    }


    public int GetHintWordAmount()
    {
        return YG2.saves.hintAmountWord;
    }

    public int GetHintLetterAmount()
    {
        return YG2.saves.hintAmountLetter;
    }
    


    private void OnLevelEndBehavior()
    {
        YG2.saves.currentLevel = 1;
        YG2.saves.hintAmountWord = 2;
        YG2.SaveProgress();

    }

   



}