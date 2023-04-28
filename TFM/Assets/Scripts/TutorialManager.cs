using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] Sprite circleMaskSprite;
    [SerializeField] Sprite squareMaskSprite;

    private bool activeTutorial;

    public void ActivateTutorialPoint()
    {
        if (!activeTutorial)
            return;

        tutorialCanvas.SetActive(true);
    }

    public void CloseTutorialPoint()
    {
        tutorialCanvas.SetActive(false);
    }

    public void SkipTutorial()
    {
        activeTutorial = false;
        tutorialCanvas.SetActive(false);
    }
}

public struct TutorialPoint
{
    public string[] tutorialText;
}
