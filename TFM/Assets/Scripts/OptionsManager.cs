using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance;
    public static int selectedLanguage;
    public static Action OnLanguageChanged;

    private static int maxLocaleId;

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
        StartCoroutine(SetLocale());
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

    [ContextMenu("Clear Language PlayerPrefs")]
    public void ClearLanguagePlayerPrefs()
    {
        PlayerPrefs.DeleteKey("SelectedLanguage");
    }
}
