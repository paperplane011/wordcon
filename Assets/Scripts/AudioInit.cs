using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AudioInit : MonoBehaviour
{

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(Clicked);
    }

    void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }


    private void Clicked()
    {
        SoundManager.Instance.Play(SoundManager.SoundInfoName.letterButtonClicked);
    }

}
