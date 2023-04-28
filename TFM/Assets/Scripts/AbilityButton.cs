using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] TMP_Text abilityText;

    Button abilityButton;
    //CharacterCard character;
    Ability ability;

    public static Action<Ability> OnAbilitySelected;

    private void Awake()
    {
        //character = GetComponentInParent<CharacterCard>();
        abilityButton = GetComponent<Button>();
        abilityButton.onClick.AddListener(SelectAbility);
    }

    public void AssignAbility(Ability abilityToAssign)
    {
        ability = abilityToAssign;
        //abilityText.SetText(ability.GetEffects()[0].effectText);
        abilityText.SetText(ability.abilityText);
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
