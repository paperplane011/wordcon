using UnityEngine;

public class LetterButtonsLineConnector : MonoBehaviour
{
    #region SINGLETONE
    private static LetterButtonsLineConnector _instance;

    public static LetterButtonsLineConnector Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<LetterButtonsLineConnector>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(LetterButtonsLineConnector).Name);
                    _instance = singletonObject.AddComponent<LetterButtonsLineConnector>();

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

    // lineXY, x - origin id, y - dest id
    [SerializeField] private CanvasGroup _line12;
    [SerializeField] private CanvasGroup _line13;
    [SerializeField] private CanvasGroup _line14;
    [SerializeField] private CanvasGroup _line15;
    [SerializeField] private CanvasGroup _line23;
    [SerializeField] private CanvasGroup _line24;
    [SerializeField] private CanvasGroup _line25;
    [SerializeField] private CanvasGroup _line34;
    [SerializeField] private CanvasGroup _line35;
    [SerializeField] private CanvasGroup _line45;


    private int _lastClickedButtonID = -1;

    private void OnEnable()
    {
        LetterInput.OnWordEnterEnd += () => ResetLines();
    }

    private void OnDisable()
    {
        LetterInput.OnWordEnterEnd -= () => ResetLines();
    }

    private void Start()
    {
        ResetLines();
    }

    private void ResetLines()
    {
        _line12.alpha = 0;
        _line13.alpha = 0;
        _line14.alpha = 0;
        _line15.alpha = 0;
        _line23.alpha = 0;
        _line24.alpha = 0;
        _line25.alpha = 0;
        _line34.alpha = 0;
        _line35.alpha = 0;
        _line45.alpha = 0;

        _lastClickedButtonID = -1;
    }

    public void EnableLineToButton(int id)
    {
        if (_lastClickedButtonID == -1)
        {
            _lastClickedButtonID = id;
            return;
        }

        if (_lastClickedButtonID == 1)
        {
            if (id == 2) _line12.alpha = 1;
            else if (id == 3) _line13.alpha = 1;
            else if (id == 4) _line14.alpha = 1;
            else if (id == 5) _line15.alpha = 1;
        }
        else if (_lastClickedButtonID == 2)
        {
            if (id == 1) _line12.alpha = 1;
            else if (id == 3) _line23.alpha = 1;
            else if (id == 4) _line24.alpha = 1;
            else if (id == 5) _line25.alpha = 1;
        }
        else if (_lastClickedButtonID == 3)
        {
            if (id == 1) _line13.alpha = 1;
            else if (id == 2) _line23.alpha = 1;
            else if (id == 4) _line34.alpha = 1;
            else if (id == 5) _line35.alpha = 1;
        }
        else if (_lastClickedButtonID == 4)
        {
            if (id == 1) _line14.alpha = 1;
            else if (id == 2) _line24.alpha = 1;
            else if (id == 3) _line34.alpha = 1;
            else if (id == 5) _line45.alpha = 1;
        }
        else if (_lastClickedButtonID == 5)
        {
            if (id == 1) _line15.alpha = 1;
            else if (id == 2) _line25.alpha = 1;
            else if (id == 3) _line35.alpha = 1;
            else if (id == 4) _line45.alpha = 1;
        }

        _lastClickedButtonID = id;

    }

    


}
