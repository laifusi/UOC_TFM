using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance;
    public static int selectedLanguage;
    public static Action OnLanguageChanged;
    public static Action OnMusicVolChanged;
    public static Action OnFXVolChanged;

    [SerializeField] AudioMixer mixer;
    [SerializeField] float maxSoundValue = 10, minSoundValue = -40;

    private static int maxLocaleId;
    private static int currentMusicVolume;
    private static int currentFXVolume;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        maxLocaleId = LocalizationSettings.AvailableLocales.Locales.Count - 1;
        selectedLanguage = PlayerPrefs.GetInt("SelectedLanguage", 0);
        currentFXVolume = PlayerPrefs.GetInt("FXVolume", 50);
        currentMusicVolume = PlayerPrefs.GetInt("MusicVolume", 50);
        StartCoroutine(SetLocale());
    }

    private void Start()
    {
        SetFXVolume();
        SetMusicVolume();
    }

    public void ChangeLanguage(bool next)
    {
        if(maxLocaleId < 0)
            maxLocaleId = LocalizationSettings.AvailableLocales.Locales.Count - 1;

        if (next && selectedLanguage < maxLocaleId)
            selectedLanguage++;
        else if (next)
            selectedLanguage = 0;
        else if (selectedLanguage == 0)
            selectedLanguage = maxLocaleId;
        else
            selectedLanguage--;

        StartCoroutine(SetLocale());
    }

    IEnumerator SetLocale()
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedLanguage];
        PlayerPrefs.SetInt("SelectedLanguage", selectedLanguage);
        OnLanguageChanged?.Invoke();
    }

    public void ChangeMusicVolume(bool more)
    {
        if (more)
        {
            currentMusicVolume = currentMusicVolume + 5 > 100 ? 100 : currentMusicVolume + 5;
        }
        else
        {
            currentMusicVolume = currentMusicVolume - 5 < 0 ? 0 : currentMusicVolume - 5;
        }

        SetMusicVolume();
    }

    private void SetMusicVolume()
    {
        float soundValue = currentMusicVolume * (maxSoundValue - minSoundValue) / 100 + minSoundValue;
        mixer.SetFloat("musicVolume", soundValue);
        PlayerPrefs.SetInt("MusicVolume", currentMusicVolume);
        OnMusicVolChanged?.Invoke();
    }

    public void ChangeFXVolume(bool more)
    {
        if (more)
        {
            currentFXVolume = currentFXVolume + 5 > 100 ? 100 : currentFXVolume + 5;
        }
        else
        {
            currentFXVolume = currentFXVolume - 5 < 0 ? 0 : currentFXVolume - 5;
        }

        SetFXVolume();
    }

    private void SetFXVolume()
    {
        float soundValue = currentFXVolume * (maxSoundValue - minSoundValue) / 100 + minSoundValue;
        mixer.SetFloat("fxVolume", soundValue);
        PlayerPrefs.SetInt("FXVolume", currentFXVolume);
        OnFXVolChanged?.Invoke();
    }

    #if UNITY_EDITOR
        [ContextMenu("Clear Language PlayerPrefs")]
        public void ClearLanguagePlayerPrefs()
        {
            PlayerPrefs.DeleteKey("SelectedLanguage");
        }

        [ContextMenu("Clear Volume PlayerPrefs")]
        public void ClearVolumePlayerPrefs()
        {
            PlayerPrefs.DeleteKey("MusicVolume");
            PlayerPrefs.DeleteKey("FXVolume");
        }
    #endif
}
