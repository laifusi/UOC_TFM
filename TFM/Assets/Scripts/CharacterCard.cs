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
    [SerializeField] Transform equipmentHolder;
    [SerializeField] EquipmentCard equipmentCardPrefab;

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

        //* Temporary fix to update equipment cards on character cards
        foreach(Transform child in equipmentHolder)
        {
            Destroy(child);
        }

        List<EquipmentCardSO> equipmentCards = card.GetEquipment();
        foreach (EquipmentCardSO equipment in equipmentCards)
        {
            EquipCard(equipment);
        }
        //*//
    }

    private void EquipCard(EquipmentCardSO equipment)
    {
        EquipmentCard equipCard = Instantiate(equipmentCardPrefab, equipmentHolder);
        equipCard.PaintCard(equipment);
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

    public void AddEquipmentCard(EquipmentCardSO equipment)
    {
        card.AddEquipment(equipment);
        EquipCard(equipment);
    }

    private void OnEnable()
    {
        UpdateUI();
    }
}
