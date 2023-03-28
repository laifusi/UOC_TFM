using UnityEngine;

[CreateAssetMenu(fileName = "New Character Card", menuName = "Cards/Character", order = 1)]
public class CharacterCardSO : CardSO
{
    public float baseHealth;
    public float baseAttack;
    public float baseShield;
    public int baseCoins;

    private float currentHealth;
    private float currentAttack;
    private float currentShield;
    private int currentCoins;
}
