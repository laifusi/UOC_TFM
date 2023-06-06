using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] Sprite circleMaskSprite;
    [SerializeField] Sprite squareMaskSprite;
    [SerializeField] Sprite doubleSquareMaskSprite;
    [SerializeField] Sprite shopSquaresMaskSprite;
    [SerializeField] Transform textBlock;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject lastButton;
    [SerializeField] RectTransform maskTransform;
    [SerializeField] Image maskImage;

    private bool activeTutorial;
    private TutorialPoint currentTutorialPoint;
    private TutorialLine.MaskConfiguration currentMask;

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
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        textBlock.position = currentTutorialPoint.GetTextBlockPos();
        if (currentTutorialPoint.IsMasked())
        {
            currentMask = currentTutorialPoint.GetMaskConfiguration();
            maskTransform.anchoredPosition = new Vector3(currentMask.xPos, currentMask.yPos, 0);
            maskTransform.sizeDelta = new Vector2(currentMask.width, currentMask.height);
            switch (currentMask.type)
            {
                case TutorialLine.MaskType.circle:
                    maskImage.sprite = circleMaskSprite;
                    break;
                case TutorialLine.MaskType.square:
                    maskImage.sprite = squareMaskSprite;
                    break;
                case TutorialLine.MaskType.doubleSquare:
                    maskImage.sprite = doubleSquareMaskSprite;
                    break;
                case TutorialLine.MaskType.shopSquares:
                    maskImage.sprite = shopSquaresMaskSprite;
                    break;
            }
        }
        else
        {
            maskTransform.sizeDelta = Vector2.zero;
        }
    }

    public void PlayPreviousLine()
    {
        tutorialText.SetText(currentTutorialPoint.GetPreviousLine());
        UpdateUI();
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

    public TutorialLine.MaskConfiguration GetMaskConfiguration()
    {
        return lines[currentLine - 1].mask;
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

    public bool IsMasked()
    {
        return lines[currentLine - 1].isMasked;
    }
}

[Serializable]
public struct TutorialLine
{
    public LocalizedString text;
    public Transform textBlockPosition;
    public bool isMasked;
    public MaskConfiguration mask;

    [Serializable]
    public struct MaskConfiguration
    {
        public MaskType type;
        public float xPos, yPos;
        public float width, height;
    }

    public enum MaskType
    {
        circle, square, doubleSquare, shopSquares
    }
}
