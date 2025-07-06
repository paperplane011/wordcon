using UnityEngine;

public class LetterCircle : MonoBehaviour
{
    
    [SerializeField] private int _letterButtonsToSpawn;
    [SerializeField] private GameObject _letterButtonGO;
    [SerializeField] private string _charsToSpawn;

    private void Start()
    {

        for (int i = 0; i < _letterButtonsToSpawn; i++)
        {
            var newLetterButtonGO = Instantiate(_letterButtonGO, transform);
            var newLetterButton = newLetterButtonGO.GetComponent<LetterButton>();
            newLetterButton.SetID(i);
            newLetterButton.SetButtonChar(_charsToSpawn[i]);
            
        }

    }

   




    
}
