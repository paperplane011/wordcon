using UnityEngine;

public class LetterCircle : MonoBehaviour
{
    
    
    [SerializeField] private GameObject _letterButtonGO;

    


    private void Start()
    {
        string letterButtonsToSpawn = SquareManager.Instance.GetLettersForLetterCircle();
        int numOfLettersToSpawn = letterButtonsToSpawn.Length;

        for (int i = 0; i < numOfLettersToSpawn; i++)
        {
            var newLetterButtonGO = Instantiate(_letterButtonGO, transform);
            var newLetterButton = newLetterButtonGO.GetComponent<LetterButton>();
            
            newLetterButton.SetID(i);
            newLetterButton.SetButtonChar(letterButtonsToSpawn[i]);

        }

    }

   




    
}
