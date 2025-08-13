using UnityEngine;

public class LetterButtonsLineConnector7 : MonoBehaviour
{
    

    // lineXY, x - origin id, y - dest id
    [SerializeField] private CanvasGroup _line12;
    [SerializeField] private CanvasGroup _line13;
    [SerializeField] private CanvasGroup _line14;
    [SerializeField] private CanvasGroup _line15;
    [SerializeField] private CanvasGroup _line16;
    [SerializeField] private CanvasGroup _line17;
    [SerializeField] private CanvasGroup _line23;
    [SerializeField] private CanvasGroup _line24;
    [SerializeField] private CanvasGroup _line25;
    [SerializeField] private CanvasGroup _line26;
    [SerializeField] private CanvasGroup _line27;
    [SerializeField] private CanvasGroup _line34;
    [SerializeField] private CanvasGroup _line35;
    [SerializeField] private CanvasGroup _line36;
    [SerializeField] private CanvasGroup _line37;
    [SerializeField] private CanvasGroup _line45;
    [SerializeField] private CanvasGroup _line46;
    [SerializeField] private CanvasGroup _line47;
    [SerializeField] private CanvasGroup _line56;
    [SerializeField] private CanvasGroup _line57;
    [SerializeField] private CanvasGroup _line67;


    private int _lastClickedButtonID = -1;

    private void OnEnable()
    {
        LetterInput.OnWordEnterEnd += ResetLines;
    }

    private void OnDisable()
    {
        LetterInput.OnWordEnterEnd -= ResetLines;
    }

    private void Start()
    {
        ResetLines();
    }

    public void ResetLines()
    {
        _line12.alpha = 0;
        _line13.alpha = 0;
        _line14.alpha = 0;
        _line15.alpha = 0;
        _line16.alpha = 0;
        _line17.alpha = 0;
        _line23.alpha = 0;
        _line24.alpha = 0;
        _line25.alpha = 0;
        _line26.alpha = 0;
        _line27.alpha = 0;
        _line34.alpha = 0;
        _line35.alpha = 0;
        _line36.alpha = 0;
        _line37.alpha = 0;
        _line45.alpha = 0;
        _line46.alpha = 0;
        _line47.alpha = 0;
        _line56.alpha = 0;
        _line57.alpha = 0;
        _line67.alpha = 0;

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
            else if (id == 6) _line16.alpha = 1;
            else if (id == 7) _line17.alpha = 1;
        }
        else if (_lastClickedButtonID == 2)
        {
            if (id == 1) _line12.alpha = 1;
            else if (id == 3) _line23.alpha = 1;
            else if (id == 4) _line24.alpha = 1;
            else if (id == 5) _line25.alpha = 1;
            else if (id == 6) _line26.alpha = 1;
            else if (id == 7) _line27.alpha = 1;
        }
        else if (_lastClickedButtonID == 3)
        {
            if (id == 1) _line13.alpha = 1;
            else if (id == 2) _line23.alpha = 1;
            else if (id == 4) _line34.alpha = 1;
            else if (id == 5) _line35.alpha = 1;
            else if (id == 6) _line36.alpha = 1;
            else if (id == 7) _line37.alpha = 1;
        }
        else if (_lastClickedButtonID == 4)
        {
            if (id == 1) _line14.alpha = 1;
            else if (id == 2) _line24.alpha = 1;
            else if (id == 3) _line34.alpha = 1;
            else if (id == 5) _line45.alpha = 1;
            else if (id == 6) _line46.alpha = 1;
            else if (id == 7) _line47.alpha = 1;
        }
        else if (_lastClickedButtonID == 5)
        {
            if (id == 1) _line15.alpha = 1;
            else if (id == 2) _line25.alpha = 1;
            else if (id == 3) _line35.alpha = 1;
            else if (id == 4) _line45.alpha = 1;
            else if (id == 6) _line56.alpha = 1;
            else if (id == 7) _line57.alpha = 1;
        }
        else if (_lastClickedButtonID == 6)
        {
            if (id == 1) _line16.alpha = 1;
            else if (id == 2) _line26.alpha = 1;
            else if (id == 3) _line36.alpha = 1;
            else if (id == 4) _line46.alpha = 1;
            else if (id == 5) _line56.alpha = 1;
            else if (id == 7) _line67.alpha = 1;
        }
        else if (_lastClickedButtonID == 7)
        {
            if (id == 1) _line17.alpha = 1;
            else if (id == 2) _line27.alpha = 1;
            else if (id == 3) _line37.alpha = 1;
            else if (id == 4) _line47.alpha = 1;
            else if (id == 5) _line57.alpha = 1;
            else if (id == 6) _line67.alpha = 1;
        }

        _lastClickedButtonID = id;

    }

    


}
