using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] TMP_Text coinsText;
    [SerializeField] Transform popUpTransform;

    private List<CharacterCardSO> activeCharacters = new List<CharacterCardSO>();
    private int totalCoins;

    private static Action OnCoinsChange;

    public static CoinManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        OnCoinsChange += UpdateCoinsText;
        CharacterCardSO.OnCoinsChange += UpdateTotalCoins;
    }

    public Transform GetPopUpTransform()
    {
        return popUpTransform;
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

    public bool BuyCard(EquipmentCardSO card)
    {
        if(CanBeBought(card))
        {
            totalCoins -= card.GetCost();
            OnCoinsChange?.Invoke();
            return true;
        }
        return false;
    }

    public bool CanBeBought(EquipmentCardSO card)
    {
        return card.GetCost() <= totalCoins;
    }

    private void OnDestroy()
    {
        OnCoinsChange -= UpdateCoinsText;
    }
}
