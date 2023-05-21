using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTutorialTrigger : TutorialTrigger
{
    private bool isActive;

    private void Start()
    {
        isActive = true;
    }

    public void CheckForDeath(List<CharacterCardSO> characters)
    {
        if (!isActive)
            return;

        foreach(CharacterCardSO character in characters)
        {
            if (character.IsFrozen)
            {
                isActive = false;
                TriggerTutorial();
            }
        }
    }
}
