using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Card", menuName = "Cards/Character", order = 1)]
public class CharacterCardSO : CardSO
{
    [SerializeField] float baseHealth;
    [SerializeField] float baseAttack;
    [SerializeField] float baseShield;
    [SerializeField] int baseCoins;

    private float currentHealth;
    private float currentAttack;
    private float currentShield;
    private int currentCoins;
    private List<EquipmentCardSO> equipmentCards = new List<EquipmentCardSO>();

    public void ResetCard()
    {
        currentHealth = baseHealth;
        currentShield = baseShield;
        currentAttack = baseAttack;
        currentCoins = baseCoins;
        equipmentCards.Clear();
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
        }
    }

    public void AddEquipment(EquipmentCardSO equipment)
    {
        equipmentCards.Add(equipment);
    }
}
