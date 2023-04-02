using UnityEngine;

[CreateAssetMenu(fileName = "New Event Card", menuName = "Cards/Event", order = 2)]
public class EventCardSO : CardSO
{
    [SerializeField] float health;
    [SerializeField] float attack;

    public float GetHealth()
    {
        return health;
    }

    public float GetAttack()
    {
        return attack;
    }
}
