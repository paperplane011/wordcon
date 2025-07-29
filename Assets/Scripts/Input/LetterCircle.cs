using System.Collections.Generic;
using UnityEngine;

public class LetterCircle : MonoBehaviour
{


    [SerializeField] private GameObject _letterButtonGO;
    private List<GameObject> _spawnedLetterButtonsList = new();


    void OnEnable()
    {
        SquareManager.Instance.OnLevelSOSetupped += SpawnLetterButtons;
    }

    void OnDisable()
    {
        SquareManager.Instance.OnLevelSOSetupped -= SpawnLetterButtons;
    }




    private void SpawnLetterButtons()
    {
        DeleteExistingButtons();

        string letterButtonsToSpawn = SquareManager.Instance.GetLettersForLetterCircle();
        int numOfLettersToSpawn = letterButtonsToSpawn.Length;

        int id = 1;
        for (int i = 0; i < numOfLettersToSpawn; i++)
        {
            var newLetterButtonGO = Instantiate(_letterButtonGO, transform);
            _spawnedLetterButtonsList.Add(newLetterButtonGO);
            var newLetterButton = newLetterButtonGO.GetComponent<LetterButton>();

            newLetterButton.SetID(id);
            id++;
            newLetterButton.SetButtonChar(letterButtonsToSpawn[i]);

        }

    }

    private void DeleteExistingButtons()
    {
        if (_spawnedLetterButtonsList.Count == 0) return;
        foreach (var button in _spawnedLetterButtonsList)
        {
            Destroy(button);
        }

        _spawnedLetterButtonsList.Clear();
    }

   




    
}
