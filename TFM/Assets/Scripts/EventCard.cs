using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventCard : Card<EventCardSO>
{
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text attackText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text effectText;

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
        effectText.text = card.GetEffects()[0].effectText;
    }

    public Effect[] GetEffects()
    {
        return card.GetEffects();
    }
}
