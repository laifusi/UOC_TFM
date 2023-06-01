using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] Sprite circleMaskSprite;
    [SerializeField] Sprite squareMaskSprite;
    [SerializeField] Transform textBlock;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject lastButton;

    private bool activeTutorial;
    private TutorialPoint currentTutorialPoint;

    private void Start()
    {
        TutorialTrigger.OnTutorialTriggered += ActivateTutorialPoint;
        OptionsManager.OnLanguageChanged += UpdateLanguage;
    }

    private void UpdateLanguage()
    {
        if(tutorialCanvas.activeInHierarchy)
            tutorialText.SetText(currentTutorialPoint.GetCurrentLine());
    }

    public void ActivateTutorialPoint(TutorialPoint tutorial)
    {
        tutorialCanvas.SetActive(true);
        currentTutorialPoint = tutorial;
        PlayNextLine();
    }

    public void PlayNextLine()
    {
        tutorialText.SetText(currentTutorialPoint.GetNextLine());
        UpdateButtons();

        if (currentTutorialPoint.IsDone())
        {
            CloseTutorialPoint();
        }
        else
        {
            textBlock.position = currentTutorialPoint.GetTextBlockPos();
        }
    }

    public void PlayPreviousLine()
    {
        tutorialText.SetText(currentTutorialPoint.GetPreviousLine());
        textBlock.position = currentTutorialPoint.GetTextBlockPos();
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        backButton.SetActive(!currentTutorialPoint.IsFirst());
        nextButton.SetActive(!currentTutorialPoint.IsLast());
        lastButton.SetActive(currentTutorialPoint.IsLast());
    }

    public void CloseTutorialPoint()
    {
        tutorialCanvas.SetActive(false);
    }

    public void SkipTutorial()
    {
        tutorialCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        TutorialTrigger.OnTutorialTriggered -= ActivateTutorialPoint;
        OptionsManager.OnLanguageChanged -= UpdateLanguage;
    }
}

[Serializable]
public struct TutorialPoint
{
    public TutorialLine[] lines;

    private int currentLine;

    public string GetNextLine()
    {
        currentLine++;
        if (IsDone())
        {
            return null;
        }
        string lineToShow = lines[currentLine - 1].text.GetLocalizedString();
        return lineToShow;
    }

    public string GetPreviousLine()
    {
        if (!IsFirst())
            currentLine--;
        string lineToShow = lines[currentLine - 1].text.GetLocalizedString();
        return lineToShow;
    }

    public string GetCurrentLine()
    {
        return lines[currentLine - 1].text.GetLocalizedString();
    }

    public Vector3 GetTextBlockPos()
    {
        return lines[currentLine - 1].textBlockPosition.position;
    }

    public bool IsDone()
    {
        return currentLine >= lines.Length + 1;
    }

    public bool IsFirst()
    {
        return currentLine <= 1;
    }

    public bool IsLast()
    {
        return currentLine == lines.Length;
    }
}

[Serializable]
public struct TutorialLine
{
    public LocalizedString text;
    public Transform textBlockPosition;
    public Transform maskPosition;
    public bool isMasked;
}
