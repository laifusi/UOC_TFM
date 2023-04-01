using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventCard : Card<EventCardSO>
{
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text attackText;
    [SerializeField] TMP_Text nameText;

    private float health;
    private float attack;

    public override void PaintCard(EventCardSO cardToPaint)
    {
        card = cardToPaint;
        health = cardToPaint.health;
        attack = cardToPaint.attack;
        nameText.text = cardToPaint.cardName;
        healthText.text = health.ToString();
        attackText.text = attack.ToString();
    }
}
