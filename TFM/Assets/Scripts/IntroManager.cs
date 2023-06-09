using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] MenuManager menuManager;

    private StoryPoint storyPoint;
    private bool isPaused;

    private void Start()
    {
        OptionsManager.OnLanguageChanged += UpdateText;

        storyPoint = GetComponent<StoryPoint>();
        PlayNextLine();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause(!isPaused);
        }
    }
    public void Pause(bool pause)
    {
        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);
    }

    public void PlayNextLine()
    {
        text.SetText(storyPoint.GetNextLine());

        if (storyPoint.IsDone())
        {
            menuManager.StartGameScene();
        }
    }

    private void UpdateText()
    {
        text.SetText(storyPoint.GetCurrentLine());
    }

    private void OnDestroy()
    {
        OptionsManager.OnLanguageChanged -= UpdateText;
    }
}
