using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentCard : Card<EquipmentCardSO>
{
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text effectText;

    public override void PaintCard(EquipmentCardSO cardToPaint)
    {
        card = cardToPaint;
        nameText.text = card.GetName();
        costText.text = card.GetCost().ToString();
        effectText.text = card.GetEffects()[0].effectText;
    }

    public int GetCost()
    {
        return card.GetCost();
    }
}
