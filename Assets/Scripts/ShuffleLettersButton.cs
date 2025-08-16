using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShuffleLettersButton : MonoBehaviour
{

    private Button _button;
    public static Action OnShuffleLettersButtonClicked;

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
        OnShuffleLettersButtonClicked?.Invoke();
        SoundManager.Instance.Play(SoundManager.SoundInfoName.letterButtonClicked, 0.8f);
    }
}
