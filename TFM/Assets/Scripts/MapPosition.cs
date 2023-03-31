using System;
using UnityEngine;

public class MapPosition : MonoBehaviour
{
    [SerializeField] private MapPosition[] connectedPositions;
    [SerializeField] private Deck eventDeck;

    [Header("Shop")]
    [SerializeField] private bool isShop;
    [SerializeField] private Deck shopDeck;

    [SerializeField] private bool isConnected;
    [SerializeField] private bool isCurrent;
    private SpriteRenderer positionSprite;

    public bool IsConnected { get => isConnected; set => isConnected = value; }
    public bool IsCurrent { get => isCurrent; set => isCurrent = value; }

    public static Action<MapPosition> OnPositionSelected;

    private void Awake()
    {
        positionSprite = GetComponent<SpriteRenderer>();
    }

    [ContextMenu("Update Positions")]
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
        if(isConnected)
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
}
