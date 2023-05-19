using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] TutorialPoint tutorialPoint;
    [SerializeField] bool onEnableTutorial = true;

    public static Action<TutorialPoint> OnTutorialTriggered;

    private void OnEnable()
    {
        if(onEnableTutorial && GameManager.Instance.TutorialsActive)
            TriggerTutorial();
    }

    public void TriggerTutorial()
    {
        OnTutorialTriggered?.Invoke(tutorialPoint);
        this.enabled = false;
    }
}
