using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class LetterCircle : MonoBehaviour
{


    [SerializeField] private GameObject _letterButtonGO;
    [SerializeField] private CircularLayout _circularLayout;

    [BoxGroup("5 letters settings")]
    [SerializeField] private float _radius5Letters;

    [BoxGroup("5 letters settings")]
    [SerializeField] private float _angleStep5Letters;


    [BoxGroup("6 letters settings")]
    [SerializeField] private float _radius6Letters;

    [BoxGroup("6 letters settings")]
    [SerializeField] private float _angleStep6Letters;


    [BoxGroup("7 letters settings")]
    [SerializeField] private float _radius7Letters;

    [BoxGroup("7 letters settings")]
    [SerializeField] private float _angleStep7Letters;


    private List<GameObject> _spawnedLetterButtonsList = new();


    void OnEnable()
    {
        SquareManager.Instance.OnLevelSOSetupped += SpawnLetterButtons;
    }

    void OnDisable()
    {
        SquareManager.Instance.OnLevelSOSetupped -= SpawnLetterButtons;
    }


    private string Shuffle(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        char[] chars = input.ToCharArray();
        int n = chars.Length;

        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            (chars[k], chars[n]) = (chars[n], chars[k]); // Обмен значениями
        }

        return new string(chars);
    }



    private void SpawnLetterButtons()
    {
        DeleteExistingButtons();

        string letterButtonsToSpawn = SquareManager.Instance.GetLettersForLetterCircle();
        letterButtonsToSpawn = Shuffle(letterButtonsToSpawn);
        int numOfLettersToSpawn = letterButtonsToSpawn.Length;
        SetupCircularLayout(numOfLettersToSpawn);

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

    private void SetupCircularLayout(int numOfLetters)
    {
        if (numOfLetters == 5)
        {
            _circularLayout.Set(_radius5Letters, 0, _angleStep5Letters);
        }
        else if (numOfLetters == 6)
        {
            _circularLayout.Set(_radius6Letters, 0, _angleStep6Letters);
        }
        else if (numOfLetters == 7)
        {
            _circularLayout.Set(_radius7Letters, 0, _angleStep7Letters);
        }
    }

   




    
}
