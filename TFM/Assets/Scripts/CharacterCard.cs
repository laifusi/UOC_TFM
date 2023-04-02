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
    private float currentHealth;
    private float currentAttack;
    private float currentShield;

    private void Start()
    {
        PaintCard(baseCard);
    }

    public override void PaintCard(CharacterCardSO cardToPaint)
    {
        card = cardToPaint;
        cardToPaint.ResetCard();
        currentHealth = cardToPaint.baseHealth;
        currentShield = cardToPaint.baseShield;
        currentAttack = cardToPaint.baseAttack;
        nameText.text = cardToPaint.cardName;
        healthText.text = currentHealth.ToString();
        shieldText.text = currentShield.ToString();
        attackText.text = currentAttack.ToString();
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
    }
}
