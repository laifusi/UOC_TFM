using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryPointUIControl : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject regularLineButton;
    [SerializeField] GameObject lastLineButton;

    private void Start()
    {
        GameManager.OnNewStoryLine += UpdateUI;
    }

    private void UpdateUI(string newLine, bool lastLine)
    {
        text.SetText(newLine);
        regularLineButton.SetActive(!lastLine);
        lastLineButton.SetActive(lastLine);
    }

    private void OnDestroy()
    {
        GameManager.OnNewStoryLine -= UpdateUI;
    }
}
