using System;
using UnityEngine;

public class MapPosition : MonoBehaviour
{
    [SerializeField] private MapPosition[] connectedPositions;
    [SerializeField] private Deck<EventCardSO> eventDeck;

    [Header("Shop")]
    [SerializeField] private bool isShop;
    [SerializeField] private Deck<EquipmentCardSO> shopDeck;

    [SerializeField] private bool isConnected;
    [SerializeField] private bool isCurrent;
    [SerializeField] private bool inMapState;
    [SerializeField] private CharacterCardSO foundCharacter;

    private SpriteRenderer positionSprite;

    public bool IsConnected { get => isConnected; set => isConnected = value; }
    public bool IsCurrent { get => isCurrent; set => isCurrent = value; }
    public bool IsShop => isShop;
    public bool InMapState { get => inMapState; set => inMapState = value; }

    public static Action<MapPosition> OnPositionSelected;

    private void Awake()
    {
        positionSprite = GetComponent<SpriteRenderer>();
    }

    public void UpdatePositionColor()
    {
        if(isCurrent)
        {
            positionSprite.color = Color.blue;
        }
        else if(isConnected)
        {
            positionSprite.color = Color.yellow;
        }
        else
        {
            positionSprite.color = Color.red;
        }
    }

    private void OnMouseUp()
    {
        if(inMapState && isConnected)
        {
            OnPositionSelected?.Invoke(this);
        }
    }

    public void ChangeToCurrent()
    {
        isCurrent = true;
        foreach (MapPosition connectedPos in connectedPositions)
        {
            connectedPos.IsConnected = true;
        }
    }

    public EventCardSO[] GetEvent()
    {
        return eventDeck.Draw();
    }

    public EquipmentCardSO[] GetShop()
    {
        return shopDeck.Draw();
    }

    public bool HasCharacter()
    {
        return foundCharacter != null;
    }

    public CharacterCardSO GetCharacter()
    {
        return foundCharacter;
    }
}
