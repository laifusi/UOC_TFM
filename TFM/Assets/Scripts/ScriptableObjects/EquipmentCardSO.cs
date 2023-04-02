using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Card", menuName = "Cards/Equipment", order = 3)]
public class EquipmentCardSO : CardSO
{
    [SerializeField] int cost;

    public int GetCost()
    {
        return cost;
    }
}
