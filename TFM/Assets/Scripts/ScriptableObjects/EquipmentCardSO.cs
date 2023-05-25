using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "New Equipment Card", menuName = "Cards/Equipment", order = 3)]
public class EquipmentCardSO : CardSO
{
    [SerializeField] int cost;
    [SerializeField] LocalizedString effectString;

    public int GetCost()
    {
        return cost;
    }

    public string GetEffectString()
    {
        return effectString.GetLocalizedString();
    }
}
