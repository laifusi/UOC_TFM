using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] CharacterCard[] activeCharacters;
    [SerializeField] TMP_Text coinsText;

    private static int totalCoins;

    private static Action OnCoinsChange;

    private void Start()
    {
        OnCoinsChange += UpdateCoinsText;
        UpdateTotalCoins();
    }

    private void UpdateTotalCoins()
    {
        totalCoins = 0;
        foreach (CharacterCard character in activeCharacters)
        {
            totalCoins += character.GetCoins();
        }
        Debug.Log(totalCoins);
        OnCoinsChange?.Invoke();
    }

    private void UpdateCoinsText()
    {
        Debug.Log(coinsText);
        coinsText.SetText(totalCoins.ToString());
        Debug.Log(coinsText);
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
