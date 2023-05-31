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

    private void Start()
    {
        OptionsManager.OnLanguageChanged += UpdateText;
    }

    public override void PaintCard(EquipmentCardSO cardToPaint)
    {
        card = cardToPaint;
        nameText.text = card.GetName();
        costText.text = card.GetCost().ToString();
        effectText.text = card.GetEffectString();
        affectedStatImage.sprite = IconManager.Instance.GetStatSprite(card.GetEffects()[0].affectedStat);
    }

    private void UpdateText()
    {
        effectText.text = card.GetEffectString();
    }

    public int GetCost()
    {
        return card.GetCost();
    }

    private void OnDestroy()
    {
        OptionsManager.OnLanguageChanged -= UpdateText;
    }
}
