using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadCanvasButton : MonoBehaviour
{

    [SerializeField] private CanvasType _canvasToLoad;
    [SerializeField] private bool _additive = false;

    private Button _button;


    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(LoadCanvas);
    }

    void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void LoadCanvas()
    {
        
        CanvasManager.Instance.LoadCanvasGroup(_canvasToLoad, (_additive ? LoadCanvasMode.Additive : LoadCanvasMode.Single));
    }
    



}
