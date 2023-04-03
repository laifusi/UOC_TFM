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
    [SerializeField] EventCard eventCard;
    [SerializeField] EquipmentCard[] equipmentCards;

    private GameState currentState;
    private MapPosition currentPosition;
    private Effect[] selectedAbility;
    private EquipmentCardSO selectedEquipment;

    private void Start()
    {
        ResetAllCanvases();
        MapPosition.OnPositionSelected += SelectPosition;
        UpdatePositions(initialPosition);
        ChangeToState(GameState.Map);
    }

    // Temporary fix to cards resetting on each phase on the first turn
    private void ResetAllCanvases()
    {
        mapCanvas.SetActive(true);
        eventCanvas.SetActive(true);
        abilitiesCanvas.SetActive(true);
        shopCanvas.SetActive(true);
        mapCanvas.SetActive(false);
        eventCanvas.SetActive(false);
        abilitiesCanvas.SetActive(false);
        shopCanvas.SetActive(false);
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
        EventCardSO cardEvent = currentPosition.GetEvent()[0];
        eventCard.PaintCard(cardEvent);
    }

    private void GetRandomShopCards()
    {
        EquipmentCardSO[] shopCards = currentPosition.GetShop();
        for(int i = 0; i < shopCards.Length; i++)
        {
            equipmentCards[i].PaintCard(shopCards[i]);
        }
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
        UpdatePositions(newMapPosition);

        ChangeToState(GameState.Event);
    }

    private void UpdatePositions(MapPosition newMapPosition)
    {
        foreach (MapPosition pos in mapPositions)
        {
            pos.IsConnected = false;
            pos.IsCurrent = false;
        }

        newMapPosition.ChangeToCurrent();

        foreach (MapPosition pos in mapPositions)
        {
            pos.UpdatePositionColor();
        }

        currentPosition = newMapPosition;
    }

    public void ChangeStateWithButton()
    {
        switch (currentState)
        {
            case GameState.Event:
                CharacterCard selectedCharacter = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<CharacterCard>();
                StartCoroutine(ApplyEffectsAndChangeState(selectedCharacter));
                break;
            case GameState.Abilities:
                selectedAbility = null;
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

    private IEnumerator ApplyEffectsAndChangeState(CharacterCard selectedCharacter)
    {
        Effect[] effectsToApply = eventCard.GetEffects();
        foreach (Effect effect in effectsToApply)
        {
            selectedCharacter.ApplyEffect(effect);
        }

        yield return new WaitForSeconds(1);

        ChangeToState(GameState.Abilities);
    }

    public void SelectAbility()
    {
        CharacterCard selectedCharacter = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInParent<CharacterCard>();
        selectedAbility = selectedCharacter.GetEffects();
    }

    public void ApplySelectedAbility()
    {
        if(selectedAbility != null && selectedAbility.Length > 0)
        {
            CharacterCard selectedCharacter = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<CharacterCard>();
            foreach(Effect effect in selectedAbility)
            {
                selectedCharacter.ApplyEffect(effect);
            }
        }
    }

    public void SelectEquipment()
    {
        EquipmentCard equipment = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInParent<EquipmentCard>();
        selectedEquipment = equipment.GetCardSO();
    }

    public void AssignEquipment()
    {
        CharacterCard selectedCharacter = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<CharacterCard>();
        selectedCharacter.AddEquipmentCard(selectedEquipment);
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
