using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventCard : Card<EventCardSO>
{
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text attackText;
    [SerializeField] TMP_Text nameText;

    public override void PaintCard(EventCardSO cardToPaint)
    {
        card = cardToPaint;
        UpdateUI();
    }

    private void UpdateUI()
    {
        nameText.text = card.GetName();
        healthText.text = card.GetHealth().ToString();
        attackText.text = card.GetAttack().ToString();
    }

    public Effect[] GetEffects()
    {
        return card.GetEffects();
    }
}
