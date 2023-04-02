using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterCard : Card<CharacterCardSO>
{
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text attackText;
    [SerializeField] TMP_Text shieldText;
    [SerializeField] TMP_Text nameText;

    private List<EquipmentCardSO> equipmentCards = new List<EquipmentCardSO>();

    private void Awake()
    {
        PaintCard(baseCard);
    }

    public override void PaintCard(CharacterCardSO cardToPaint)
    {
        card = cardToPaint;
        card.ResetCard();
        UpdateUI();
    }

    private void UpdateUI()
    {
        nameText.text = card.GetName();
        healthText.text = card.GetHealth().ToString();
        shieldText.text = card.GetShield().ToString();
        attackText.text = card.GetAttack().ToString();
    }

    [ContextMenu("Test effect -5 health")]
    public void TestApplyEffect()
    {
        Effect eff = new Effect
        {
            affectedStat = StatType.Health,
            affectionAmount = -5
        };
        ApplyEffect(eff);
    }

    public void ApplyEffect(Effect effect)
    {
        card.ApplyEffect(effect);
        UpdateUI();
    }

    public Effect[] GetEffects()
    {
        return card.GetEffects();
    }

    private void OnEnable()
    {
        UpdateUI();
    }
}
