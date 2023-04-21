using System;
using UnityEngine;

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
            if(outcome.detonatorStat == StatType.Attack || outcome.detonatorStat == StatType.Health)
            {
                Debug.Log("Detonator: " + outcome.detonatorStat);
                // If partial attack or full attack, we reduce character's life and send additional outcome effects;
                character.GetAttacked(attack);
                return outcome.effects;
            }
            else if(character.GetStat(outcome.detonatorStat) >= outcome.detonationValue)
            {
                Debug.Log("Detonator: " + outcome.detonatorStat);
                return outcome.effects;
            }
        }

        return null;
    }
}

[Serializable]
public struct EventOutcome
{
    public Effect[] effects;
    public StatType detonatorStat;
    public int detonationValue;
}
