using UnityEngine;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(Button))]
public class LoadCanvasButton : MonoBehaviour
{

    [SerializeField] private CanvasType _canvasToLoad;
    [SerializeField] private bool _additive = false;
    [SerializeField] private bool _showAd = false;
    private Button _button;


    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(LoadCanvasOrShowAd);
        if (_showAd) YG2.onCloseInterAdv += LoadCanvas;
    }

    void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
        if (_showAd) YG2.onCloseInterAdv -= LoadCanvas;
    }

    private void LoadCanvasOrShowAd()
    {
        if (!_showAd)
        {
            CanvasManager.Instance.LoadCanvasGroup(_canvasToLoad, (_additive ? LoadCanvasMode.Additive : LoadCanvasMode.Single));
        }
        else
        {
            YG2.InterstitialAdvShow();
        }
    }

    private void LoadCanvas()
    {
        CanvasManager.Instance.LoadCanvasGroup(_canvasToLoad, (_additive ? LoadCanvasMode.Additive : LoadCanvasMode.Single));
    }
    



}
