using UnityEngine;
using UnityEngine.UI;
using YG;


[RequireComponent(typeof(Button))]
public class AddIconButton : MonoBehaviour
{

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(Clicked);
        YG2.onGameLabelSuccess += () => Destroy(this.gameObject);
    }

    void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
        YG2.onGameLabelSuccess -= () => Destroy(this.gameObject);
    }

    void Start()
    {
        if (!YG2.gameLabelCanShow)
        {
            Destroy(this.gameObject);
        }
    }


    private void Clicked()
    {
        if (YG2.gameLabelCanShow)
        {
            YG2.GameLabelShowDialog();
        }
    }

}
