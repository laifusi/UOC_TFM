using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTutorialTrigger : TutorialTrigger
{
    public void CheckForDeath(List<CharacterCardSO> characters)
    {
        foreach(CharacterCardSO character in characters)
        {
            if (character.IsFrozen)
                TriggerTutorial();
        }
    }
}
