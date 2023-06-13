using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VolumeUIText : MonoBehaviour
{
    [SerializeField] bool isMusic;
    [SerializeField] bool isFX;

    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();

        if (isMusic)
            OptionsManager.OnMusicVolChanged += UpdateText;
        else if (isFX)
            OptionsManager.OnFXVolChanged += UpdateText;

        UpdateText();
    }

    private void UpdateText()
    {
        if (isMusic)
            text.SetText(PlayerPrefs.GetInt("MusicVolume").ToString() + "%");
        else if (isFX)
            text.SetText(PlayerPrefs.GetInt("FXVolume").ToString() + "%");
    }

    private void OnDestroy()
    {
        if (isMusic)
            OptionsManager.OnMusicVolChanged -= UpdateText;
        else if (isFX)
            OptionsManager.OnFXVolChanged -= UpdateText;
    }
}
