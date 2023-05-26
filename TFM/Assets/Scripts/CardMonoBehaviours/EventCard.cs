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
    [SerializeField] DetonatorUIController detonatorUI;

    public override void PaintCard(EventCardSO cardToPaint)
    {
        card = cardToPaint;
        UpdateUI();
        detonatorUI.UpdateUI();
    }

    private void UpdateUI()
    {
        nameText.text = card.GetName();
        healthText.text = card.GetHealth().ToString();
        attackText.text = card.GetAttack().ToString();
        //effectText.text = card.GetEventOutcomesText();
    }

    public Effect[] GetEffects()
    {
        return card.GetEffects();
    }

    public Effect[] GetEventOutcomeEffects(CharacterCard chosenCharacter)
    {
        return card.GetEventOutcomeEffects(chosenCharacter);
    }

    public string GetPossibleOutcomeInfo(CharacterCard character)
    {
        return card.GetPossibleOutcomeInfo(character);
    }

    public EventOutcome[] GetOutcomes()
    {
        return card.GetOutcomes();
    }
}
