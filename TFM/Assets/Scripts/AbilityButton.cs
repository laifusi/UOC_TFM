using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    Button abilityButton;
    CharacterCard character;

    public static Action<CharacterCard> OnAbilitySelected;

    private void Start()
    {
        character = GetComponentInParent<CharacterCard>();
        abilityButton = GetComponent<Button>();
        abilityButton.onClick.AddListener(SelectAbility);
    }

    private void SelectAbility()
    {
        OnAbilitySelected?.Invoke(character);
    }

    private void OnDestroy()
    {
        abilityButton?.onClick.RemoveAllListeners();
    }
}
