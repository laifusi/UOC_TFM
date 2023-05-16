using System;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "New Event Card", menuName = "Cards/Event", order = 2)]
public class EventCardSO : CardSO
{
    [SerializeField] float health;
    [SerializeField] float attack;
    [SerializeField] EventOutcome[] possibleEventOutcomes;

    public float GetHealth()
    {
        return health;
    }

    public float GetAttack()
    {
        return attack;
    }

    public Effect[] GetEventOutcomeEffects(CharacterCard character)
    {
        foreach(EventOutcome outcome in possibleEventOutcomes)
        {
            if (outcome.detonatorStat == StatType.Attack || outcome.detonatorStat == StatType.Health) // StatType.Health == Partial Attack, StatType.Attack == Full Attack
            {
                Debug.Log("Detonator: " + outcome.detonatorStat);
                // If partial attack or full attack, we reduce character's life and send additional outcome effects;
                character.GetAttacked(attack);
                return outcome.effects;
            }
            else if (outcome.detonatorStat == StatType.Power && outcome.powerDetonator == character.GetPower())
            {
                Debug.Log("Detonator: " + outcome.powerDetonator);
                return outcome.effects;
            }
            else if (character.GetStat(outcome.detonatorStat) >= outcome.detonationValue)
            {
                Debug.Log("Detonator: " + outcome.detonatorStat);
                return outcome.effects;
            }
        }

        return null;
    }

    public string GetEventOutcomesText()
    {
        string completeText = "";
        foreach(EventOutcome outcome in possibleEventOutcomes)
        {
            completeText += outcome.outcomeText.GetLocalizedString();
            completeText += "\n";
        }
        return completeText;
    }
}

[Serializable]
public struct EventOutcome
{
    public StatType detonatorStat;
    public int detonationValue;
    public Power powerDetonator;
    public LocalizedString outcomeText;

    public Effect[] effects;
}
