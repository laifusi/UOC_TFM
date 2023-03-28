using UnityEngine;

[CreateAssetMenu(fileName = "New Event Card", menuName = "Cards/Event", order = 2)]
public class EventCardSO : CardSO
{
    public float health;
    public float attack;
    public Rarity rarity;
}
