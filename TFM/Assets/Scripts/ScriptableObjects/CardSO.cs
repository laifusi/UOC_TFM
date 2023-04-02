using System;
using UnityEngine;

public class CardSO : ScriptableObject
{
    [SerializeField] CardType type;
    [SerializeField] string cardName;
    [SerializeField] Effect[] effects;
    [SerializeField] Rarity rarity;

    public string GetName()
    {
        return cardName;
    }

    public Effect[] GetEffects()
    {
        return effects;
    }

    public Rarity GetRarity()
    {
        return rarity;
    }
}

public enum CardType
{
    Character, Event, Equipment
}

public enum StatType
{
    Health, Attack, Shield, Coins
}

[Serializable]
public struct Effect
{
    public StatType affectedStat;
    public float affectionAmount;
    public string effectText;
}

public enum Rarity
{
    Common, Uncommon, Rare
}