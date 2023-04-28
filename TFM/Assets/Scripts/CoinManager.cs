using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] TMP_Text coinsText;

    private List<CharacterCardSO> activeCharacters = new List<CharacterCardSO>();
    private static int totalCoins;

    private static Action OnCoinsChange;

    private void Start()
    {
        OnCoinsChange += UpdateCoinsText;
    }

    public void AddCharacter(CharacterCardSO card)
    {
        activeCharacters.Add(card);
        UpdateTotalCoins();
    }

    private void UpdateTotalCoins()
    {
        totalCoins = 0;
        foreach (CharacterCardSO character in activeCharacters)
        {
            totalCoins += character.GetCoins();
        }
        OnCoinsChange?.Invoke();
    }

    private void UpdateCoinsText()
    {
        coinsText.SetText(totalCoins.ToString());
    }

    public static bool BuyCard(EquipmentCardSO card)
    {
        if(card.GetCost() < totalCoins)
        {
            totalCoins -= card.GetCost();
            OnCoinsChange?.Invoke();
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        OnCoinsChange -= UpdateCoinsText;
    }
}
