using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Linq;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Objects/LevelSO")]
public class LevelSO : ScriptableObject
{
    public int LevelNum;
    public string LevelLetters;

    // ' ' - empty square, otherwise - letter
    public CharGrid LayoutString;


    [SerializedDictionary("Word, WordPos")]
    public SerializedDictionary<string, int[]> WordsPositions = new();

    public int GetNumOfWords()
    {
        return WordsPositions.Keys.Count;
    }

#if UNITY_EDITOR
    public bool IsLevelCorrect()
    {
        foreach (var word in WordsPositions.Keys)
        {
            if (word.Length != WordsPositions[word].Count())
            {
                Debug.LogError($"Level {LevelNum}: Word \"{word}\" has too much id's for letters");
                return false;
            }

            int lettersPresent = 0;
            foreach (var letter in LevelLetters)
            {
                if (word.Contains(letter))
                {
                    lettersPresent++;
                }
            }

            if (lettersPresent < word.Length)
            {
                Debug.LogError($"Level {LevelNum}: Word \"{word}\" have false letters");
                return false;
            }

            
        }

        return true;
    }


#endif


}
