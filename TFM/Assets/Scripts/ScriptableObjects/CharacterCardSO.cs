﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Card", menuName = "Cards/Character", order = 1)]
public class CharacterCardSO : CardSO
{
    [SerializeField] float baseHealth;
    [SerializeField] float baseAttack;
    [SerializeField] float baseShield;
    [SerializeField] int baseCoins;

    [SerializeField] int baseAgility;
    [SerializeField] int baseDexterity;
    [SerializeField] int baseIntelligence;
    [SerializeField] int baseConstitution;
    [SerializeField] int baseWisdom;
    [SerializeField] int baseSocialSkills;

    [SerializeField] Ability[] availableAbilities;

    private float currentHealth;
    private float currentAttack;
    private float currentShield;
    private int currentCoins;

    private int currentAgility;
    private int currentDexterity;
    private int currentIntelligence;
    private int currentConstitution;
    private int currentWisdom;
    private int currentSocialSkills;
    private int currentAbilityLevel;
    private List<EquipmentCardSO> equipmentCards = new List<EquipmentCardSO>();
    private List<Ability> activeAbilities;
    private List<Ability> inactiveAbilities;

    public void ResetCard()
    {
        currentHealth = baseHealth;
        currentShield = baseShield;
        currentAttack = baseAttack;
        currentCoins = baseCoins;
        currentAgility = baseAgility;
        currentConstitution = baseConstitution;
        currentDexterity = baseDexterity;
        currentIntelligence = baseIntelligence;
        currentSocialSkills = baseSocialSkills;
        currentWisdom = baseWisdom;
        currentAbilityLevel = 0;
        equipmentCards.Clear();
        activeAbilities.Clear();
        inactiveAbilities.Clear();
        foreach(Ability ability in availableAbilities)
        {
            if(ability.abilityLevel == 0)
            {
                activeAbilities.Add(ability);
            }
            else
            {
                inactiveAbilities.Add(ability);
            }
        }
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetShield()
    {
        return currentShield;
    }

    public float GetAttack()
    {
        return currentAttack;
    }

    public int GetCoins()
    {
        return currentCoins;
    }

    public List<EquipmentCardSO> GetEquipment()
    {
        return equipmentCards;
    }

    public void ApplyEffect(Effect effect)
    {
        switch(effect.affectedStat)
        {
            case StatType.Health:
                currentHealth += effect.affectionAmount;
                break;
            case StatType.Shield:
                currentShield += effect.affectionAmount;
                break;
            case StatType.Attack:
                currentAttack += effect.affectionAmount;
                break;
            case StatType.Coins:
                currentCoins += (int)effect.affectionAmount;
                break;
            case StatType.Agility:
                currentAgility += (int)effect.affectionAmount;
                break;
            case StatType.Constitution:
                currentConstitution += (int)effect.affectionAmount;
                break;
            case StatType.Dexterity:
                currentDexterity += (int)effect.affectionAmount;
                break;
            case StatType.Intelligence:
                currentIntelligence += (int)effect.affectionAmount;
                break;
            case StatType.SocialSkills:
                currentSocialSkills += (int)effect.affectionAmount;
                break;
            case StatType.Wisdom:
                currentWisdom += (int)effect.affectionAmount;
                break;
        }
    }

    public void AddEquipment(EquipmentCardSO equipment)
    {
        equipmentCards.Add(equipment);
    }

    public int GetStat(StatType stat)
    {
        int valueToReturn = 0;

        switch(stat)
        {
            case StatType.Agility:
                valueToReturn = currentAgility;
                break;
            case StatType.Constitution:
                valueToReturn = currentConstitution;
                break;
            case StatType.Dexterity:
                valueToReturn = currentDexterity;
                break;
            case StatType.Intelligence:
                valueToReturn = currentIntelligence;
                break;
            case StatType.SocialSkills:
                valueToReturn = currentSocialSkills;
                break;
            case StatType.Wisdom:
                valueToReturn = currentWisdom;
                break;
        }

        return valueToReturn;
    }

    public void GetAttacked(float attack)
    {
        if(currentShield < attack)
        {
            float remainingAttack = attack - currentShield;
            currentShield = 0;
            currentHealth -= remainingAttack;

            // What happens if currentHealth <= 0 ?!
        }
        else
        {
            currentShield -= attack;
        }
    }

    public List<Ability> GetLearnableAbilities()
    {
        List<Ability> learnableAbilities = new List<Ability>();
        foreach(Ability ability in inactiveAbilities)
        {
            if(currentAbilityLevel >= ability.abilityLevel)
            {
                learnableAbilities.Add(ability);
            }
        }
        return learnableAbilities;
    }

    public void LearnAbility(Ability abilityToLearn)
    {
        inactiveAbilities.Remove(abilityToLearn);
        activeAbilities.Add(abilityToLearn);
    }
}

public struct Ability
{
    public Effect[] effects;
    public string abilityText;
    public int abilityCost;
    public int abilityLevel;

    public Effect[] GetEffects()
    {
        return effects;
    }
}
