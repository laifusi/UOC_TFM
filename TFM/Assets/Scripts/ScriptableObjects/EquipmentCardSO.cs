using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Card", menuName = "Cards/Equipment", order = 3)]
public class EquipmentCardSO : CardSO
{
    public Rarity rarity;
    public int cost;
}
