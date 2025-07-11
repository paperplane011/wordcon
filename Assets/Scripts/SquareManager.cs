using UnityEngine;



public class SquareManager : MonoBehaviour
{
    [SerializeField] private int _numToSpawn = 49;
    [SerializeField] private GameObject _squarePrefab;


    private void Start()
    {
        for (int i = 0; i < _numToSpawn; i++)
        {
            var newSquare = Instantiate(_squarePrefab, transform).GetComponent<Square>();
            newSquare.SetID(i);

            if (i % 2 == 0)
            {
                newSquare.SetEmpty(false);
                newSquare.SetLetter("П");
            }
            else
            {
                newSquare.SetEmpty(true);
            }
        }
    }



}