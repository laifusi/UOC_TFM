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
    bool abilityAssigned;

    public static Action<Ability> OnAbilitySelected;

    private void Awake()
    {
        //character = GetComponentInParent<CharacterCard>();
        abilityButton = GetComponent<Button>();
        abilityButton.onClick.AddListener(SelectAbility);
    }

    private void Start()
    {
        OptionsManager.OnLanguageChanged += UpdateUI;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if(abilityAssigned)
            abilityText.SetText(ability.abilityText.GetLocalizedString());
    }

    public void AssignAbility(Ability abilityToAssign)
    {
        ability = abilityToAssign;
        abilityAssigned = true;
        UpdateUI();
    }

    private void SelectAbility()
    {
        OnAbilitySelected?.Invoke(ability);
    }

    private void OnDestroy()
    {
        abilityButton?.onClick.RemoveAllListeners();
        OptionsManager.OnLanguageChanged -= UpdateUI;
    }
}
