using UnityEngine;
using UnityEngine.UI;
using YG;


[RequireComponent(typeof(Button))]
public class RateGameButton : MonoBehaviour
{

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(Clicked);
        YG2.onReviewSent += (bool s) => { if (s) Destroy(this.gameObject); };
    }

    void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
        YG2.onReviewSent -= (bool s) => { if (s) Destroy(this.gameObject); };
    }

    void Start()
    {
        if (!YG2.reviewCanShow)
        {
            Destroy(this.gameObject);
        }
    }


    private void Clicked()
    {
        if (YG2.reviewCanShow)
        {
            YG2.ReviewShow();
        }
    }

}
