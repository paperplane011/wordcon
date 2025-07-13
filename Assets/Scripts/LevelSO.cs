using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Objects/LevelSO")]
public class LevelSO : ScriptableObject
{
    public int LevelNum;
    public string LevelLetters;

    // ' ' - empty square, otherwise - letter
    public CharGrid LayoutString;


    [SerializedDictionary("Word, WordPos")]
    public SerializedDictionary<string, int[]> WordsPositions = new();




}
