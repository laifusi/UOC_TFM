using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryPointUIControl : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject regularLineButton;
    [SerializeField] GameObject lastLineButton;
    [SerializeField] GameObject backButton;

    private void OnEnable()
    {
        GameManager.OnNewStoryLine += UpdateUI;
    }

    private void UpdateUI(string newLine, bool lastLine, bool firstLine)
    {
        text.SetText(newLine);
        regularLineButton.SetActive(!lastLine);
        lastLineButton.SetActive(lastLine);
        backButton.SetActive(!firstLine);
    }

    private void OnDisable()
    {
        GameManager.OnNewStoryLine -= UpdateUI;
    }
}
