using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Objects/LevelSO")]
public class LevelSO : ScriptableObject
{
    public int LevelNum;

    // q - empty square, otherwise - letter
    public CharGrid LayoutString;




}
