using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    Button abilityButton;
    TMP_Text abilityText;
    CharacterCard character;

    public static Action<CharacterCard> OnAbilitySelected;

    private void Start()
    {
        character = GetComponentInParent<CharacterCard>();
        abilityButton = GetComponent<Button>();
        abilityText = GetComponent<TMP_Text>();
        abilityButton.onClick.AddListener(SelectAbility);

        abilityText.SetText(character.GetEffects()[0].effectText);
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
