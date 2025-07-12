using NaughtyAttributes;
using UnityEngine;



public class SquareManager : MonoBehaviour
{
    [SerializeField] private GameObject _squarePrefab;
    [SerializeField] private LevelSO _levelSO;


    private void Start()
    {
        SetLayout(_levelSO.LayoutString.GetGridString());

    }

    private void SetLayout(string layout)
    {
        int i = 0;
        foreach (var c in layout)
        {
            var newSquare = Instantiate(_squarePrefab, transform).GetComponent<Square>();
            newSquare.SetID(i);
            i++;

            if (c == ' ')
            {
                newSquare.SetEmpty(true);
            }
            else
            {
                newSquare.SetEmpty(false);
                newSquare.SetLetter(c.ToString());
            }
        }
    }


    [Button]
    private void MakeLevelLayout()
    {
        string layout = "";

        for (int i = 0; i < transform.childCount; i++)
        {
            Square square = transform.GetChild(i).GetComponent<Square>();

            if (square.GetIsEmpty())
            {
                layout += " ";
            }
            else
            {
                layout += square.GetLetter();
            }
        }

        Debug.Log("level layout: " + layout);
    }

    


}