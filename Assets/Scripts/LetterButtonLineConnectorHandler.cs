using UnityEngine;

public class LetterButtonsLineConnectorHandler : MonoBehaviour
{
    #region SINGLETONE
    private static LetterButtonsLineConnectorHandler _instance;

    public static LetterButtonsLineConnectorHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<LetterButtonsLineConnectorHandler>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(LetterButtonsLineConnectorHandler).Name);
                    _instance = singletonObject.AddComponent<LetterButtonsLineConnectorHandler>();

                }
            }
            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    [SerializeField] LetterButtonsLineConnector5 _letterButtonLineConnector5;
    [SerializeField] LetterButtonsLineConnector6 _letterButtonLineConnector6;
    [SerializeField] LetterButtonsLineConnector7 _letterButtonLineConnector7;

    private int _numOfLetters;

    void OnEnable()
    {
        SquareManager.Instance.OnLevelSOSetupped += SetLines;
    }

    void OnDisable()
    {
        SquareManager.Instance.OnLevelSOSetupped -= SetLines;
    }


    public void EnableLineToButton(int id)
    {
        if (_numOfLetters == 5)
        {
            _letterButtonLineConnector5.EnableLineToButton(id);
        }
        else if (_numOfLetters == 6)
        {
            _letterButtonLineConnector6.EnableLineToButton(id);
        }
        else if (_numOfLetters == 7)
        {
            _letterButtonLineConnector7.EnableLineToButton(id);
        }

    }

    public void SetLines()
    {
        _numOfLetters = SquareManager.Instance.GetLettersForLetterCircle().Length;
        _letterButtonLineConnector5.ResetLines();
        _letterButtonLineConnector6.ResetLines();
        _letterButtonLineConnector7.ResetLines();
        
    }

  


}
