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

    private bool activeTutorial;
    private TutorialPoint currentTutorialPoint;

    private void Start()
    {
        TutorialTrigger.OnTutorialTriggered += ActivateTutorialPoint;
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

        if (currentTutorialPoint.IsDone())
        {
            CloseTutorialPoint();
        }
        else
        {
            textBlock.position = currentTutorialPoint.GetTextBlockPos();
        }
    }

    public void CloseTutorialPoint()
    {
        tutorialCanvas.SetActive(false);
    }

    public void SkipTutorial()
    {
        tutorialCanvas.SetActive(false);
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

    public Vector3 GetTextBlockPos()
    {
        return lines[currentLine - 1].textBlockPosition.position;
    }

    public bool IsDone()
    {
        return currentLine >= lines.Length + 1;
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
