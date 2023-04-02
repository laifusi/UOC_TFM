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

    public void ResetCard()
    {
        currentHealth = baseHealth;
        currentShield = baseShield;
        currentAttack = baseAttack;
        currentCoins = baseCoins;
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
}
