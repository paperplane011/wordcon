using UnityEngine;

[System.Serializable]
public class CharGrid
{
    public const int Size = 7;
    
    [SerializeField] 
    private string _gridData = new string('q', Size * Size);
    
    public char this[int x, int y]
    {
        get => _gridData[y * Size + x];
        set
        {
            char[] chars = _gridData.ToCharArray();
            chars[y * Size + x] = value;
            _gridData = new string(chars);
        }
    }
    
    public string GetGridString() => _gridData;
    public void SetGridString(string value) => _gridData = value;
}