using UnityEngine;
using YG;



public class Tutorial : MonoBehaviour
{
    [SerializeField] private CanvasGroup _tutorialCanvasGroup;


    void Start()
    {
        if (YG2.saves.currentLevel == 1)
        {
            _tutorialCanvasGroup.alpha = 1f;
            _tutorialCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            DisableTutorial();
        }
    }

    public void DisableTutorial()
    {
        _tutorialCanvasGroup.alpha = 0f;
            _tutorialCanvasGroup.blocksRaycasts = false;
    }



}