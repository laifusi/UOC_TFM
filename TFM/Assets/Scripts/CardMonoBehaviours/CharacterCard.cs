using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : Card<CharacterCardSO>
{
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text attackText;
    [SerializeField] TMP_Text shieldText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Transform equipmentHolder;
    [SerializeField] EquipmentCard equipmentCardPrefab;
    [SerializeField] Button button;
    [SerializeField] Transform abilitiesHolder;

    /*private void Awake()
    {
        PaintCard(baseCard);
    }*/

    public override void PaintCard(CharacterCardSO cardToPaint)
    {
        card = cardToPaint;
        card.ResetCard();
        UpdateUI();
        UpdateEquipmentUI();
    }

    private void UpdateUI()
    {
        nameText.text = card.GetName();
        healthText.text = card.GetHealth().ToString();
        shieldText.text = card.GetShield().ToString();
        attackText.text = card.GetAttack().ToString();
    }

    // Temporary fix to update equipment cards on character cards
    private void UpdateEquipmentUI()
    {
        foreach (Transform child in equipmentHolder)
        {
            Destroy(child.gameObject);
        }

        List<EquipmentCardSO> equipmentCards = card.GetEquipment();
        foreach (EquipmentCardSO equipment in equipmentCards)
        {
            EquipCard(equipment);
        }
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
        bool isBought = CoinManager.BuyCard(equipment);
        if (isBought)
        {
            card.AddEquipment(equipment);
            EquipCard(equipment);

            Effect[] effects = equipment.GetEffects();
            foreach (Effect effect in effects)
            {
                ApplyEffect(effect);
            }
        }
    }

    public int GetCoins()
    {
        return card.GetCoins();
    }

    public int GetStat(StatType statType)
    {
        return card.GetStat(statType);
    }

    public void GetAttacked(float attack)
    {
        card.GetAttacked(attack);
    }

    public void ActivateButtons(GameState state, GameManager manager)
    {
        switch(state)
        {
            case GameState.Event:
                button.enabled = true;
                button.onClick.AddListener(manager.ChangeStateWithButton);
                break;
            case GameState.Abilities:
                button.enabled = true;
                button.onClick.AddListener(manager.ApplySelectedAbility);
                break;
            case GameState.Shop:
                button.enabled = true;
                button.onClick.AddListener(manager.AssignEquipment);
                break;
        }
    }

    public List<Ability> GetActiveAbilities()
    {
        return card.GetActiveAbilities();
    }

    public Transform GetAbilitiesParent()
    {
        return abilitiesHolder;
    }

    private void OnEnable()
    {
        if(card != null)
        {
            UpdateUI();
            UpdateEquipmentUI();
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
