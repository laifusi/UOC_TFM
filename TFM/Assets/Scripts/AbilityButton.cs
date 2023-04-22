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
    //CharacterCard character;
    Ability ability;

    public static Action<Ability> OnAbilitySelected;

    private void Start()
    {
        //character = GetComponentInParent<CharacterCard>();
        abilityButton = GetComponent<Button>();
        abilityText = GetComponent<TMP_Text>();
        abilityButton.onClick.AddListener(SelectAbility);
    }

    public void AssignAbility(Ability abilityToAssign)
    {
        ability = abilityToAssign;
        abilityText.SetText(ability.GetEffects()[0].effectText);
    }

    private void SelectAbility()
    {
        OnAbilitySelected?.Invoke(ability);
    }

    private void OnDestroy()
    {
        abilityButton?.onClick.RemoveAllListeners();
    }
}
