using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class LearnableAbilityUI : MonoBehaviour
{
    [SerializeField] TMP_Text abilityText;
    [SerializeField] TMP_Text costText;
    [SerializeField] LocalizedString costTextString;
    [SerializeField] Button learnButton;

    private Ability assignedAbility;
    private CharacterCard assignedCharacter;

    private void Start()
    {
        learnButton.onClick.AddListener(() => assignedCharacter.LearnAbility(assignedAbility));
    }

    public void AssignAbility(Ability ability, CharacterCard character)
    {
        assignedCharacter = character;
        assignedAbility = ability;
        UpdateUI();
    }

    private void UpdateUI()
    {
        abilityText.SetText(assignedAbility.abilityText);
        costTextString.Arguments = new object[] { assignedAbility.abilityCost };
        costText.SetText(costTextString.GetLocalizedString());
    }

    private void OnDestroy()
    {
        learnButton.onClick.RemoveAllListeners();
    }
}
