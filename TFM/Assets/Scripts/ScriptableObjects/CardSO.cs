using System;
using UnityEngine;

public class CardSO : ScriptableObject
{
    public CardType type;
    public string cardName;
    public Effect[] effects;
    public Rarity rarity;
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