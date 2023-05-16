using System;
using UnityEngine;
using UnityEngine.Localization;

public class CardSO : ScriptableObject
{
    [SerializeField] CardType type;
    [SerializeField] LocalizedString cardName;
    [SerializeField] Effect[] effects;
    [SerializeField] Rarity rarity;

    public string GetName()
    {
        return cardName.GetLocalizedString();
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
    Health, Attack, Shield, Coins, Intelligence, SocialSkills, Dexterity, Agility, Power
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