using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MapPosition[] mapPositions;
    [SerializeField] MapPosition initialPosition;
    [SerializeField] GameObject mapCanvas;
    [SerializeField] GameObject eventCanvas;
    [SerializeField] GameObject abilitiesCanvas;
    [SerializeField] GameObject shopCanvas;

    private GameState currentState;
    private MapPosition currentPosition;

    private void Start()
    {
        MapPosition.OnPositionSelected += SelectPosition;
        SelectPosition(initialPosition);
        ChangeToState(GameState.Map);
    }

    private void ChangeToState(GameState state)
    {
        EndCurrentState();
        currentState = state;
        StartCurrentState();
    }

    private void StartCurrentState()
    {
        switch(currentState)
        {
            case GameState.Map:
                ActivateMap(true);
                ActivateCanvas(true, mapCanvas);
                break;
            case GameState.Event:
                ActivateCanvas(true, eventCanvas);
                GetRandomEvent();
                break;
            case GameState.Abilities:
                ActivateCanvas(true, abilitiesCanvas);
                break;
            case GameState.Shop:
                ActivateCanvas(true, shopCanvas);
                GetRandomShopCards();
                break;
        }
    }

    private void ActivateCanvas(bool activate, GameObject canvas)
    {
        canvas.SetActive(activate);
    }

    private void ActivateMap(bool activate)
    {
        foreach(MapPosition pos in mapPositions)
        {
            pos.InMapState = activate;
        }
    }

    private void GetRandomEvent()
    {
        CardSO cardEvent = currentPosition.GetEvent()[0];
        Debug.Log("Event: " + cardEvent.cardName);
    }

    private void GetRandomShopCards()
    {
        CardSO[] shopCards = currentPosition.GetShop();
        foreach(CardSO card in shopCards)
            Debug.Log("Shop: " + card.cardName);
    }

    private void EndCurrentState()
    {
        switch (currentState)
        {
            case GameState.Map:
                ActivateMap(false);
                ActivateCanvas(false, mapCanvas);
                break;
            case GameState.Event:
                ActivateCanvas(false, eventCanvas);
                break;
            case GameState.Abilities:
                ActivateCanvas(false, abilitiesCanvas);
                break;
            case GameState.Shop:
                ActivateCanvas(false, shopCanvas);
                break;
        }
    }

    private void SelectPosition(MapPosition newMapPosition)
    {
        foreach(MapPosition pos in mapPositions)
        {
            pos.IsConnected = false;
            pos.IsCurrent = false;
        }

        newMapPosition.ChangeToCurrent();

        foreach(MapPosition pos in mapPositions)
        {
            pos.UpdatePositionColor();
        }

        currentPosition = newMapPosition;

        ChangeToState(GameState.Event);
    }

    public void ChangeStateWithButton()
    {
        switch(currentState)
        {
            case GameState.Event:
                ChangeToState(GameState.Abilities);
                break;
            case GameState.Abilities:
                if (currentPosition.IsShop)
                    ChangeToState(GameState.Shop);
                else
                    ChangeToState(GameState.Map);
                break;
            case GameState.Shop:
                ChangeToState(GameState.Map);
                break;
        }
    }

    private void OnDestroy()
    {
        MapPosition.OnPositionSelected -= SelectPosition;
    }
}

enum GameState
{
    Map, Event, Abilities, Shop
}
