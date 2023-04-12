using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentCard : Card<EquipmentCardSO>
{
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;

    public override void PaintCard(EquipmentCardSO cardToPaint)
    {
        card = cardToPaint;
        nameText.text = card.GetName();
        costText.text = card.GetCost().ToString();
    }

    public int GetCost()
    {
        return card.GetCost();
    }
}
