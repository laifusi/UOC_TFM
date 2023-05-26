using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentCard : Card<EquipmentCardSO>
{
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text effectText;
    [SerializeField] Image affectedStatImage;

    public override void PaintCard(EquipmentCardSO cardToPaint)
    {
        card = cardToPaint;
        nameText.text = card.GetName();
        costText.text = card.GetCost().ToString();
        effectText.text = card.GetEffectString();
        affectedStatImage.sprite = IconManager.Instance.GetStatSprite(card.GetEffects()[0].affectedStat);
    }

    public int GetCost()
    {
        return card.GetCost();
    }
}
