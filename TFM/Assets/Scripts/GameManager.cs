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
    [SerializeField] GameObject learningCanvas;
    [SerializeField] GameObject newCharacterCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] EventCard eventCard;
    [SerializeField] EquipmentCard[] equipmentCards;
    [SerializeField] CharacterLayoutController[] characterCardsLayouts;
    [SerializeField] CharacterCard characterCardPrefab;
    [SerializeField] CharacterCard newCharacterCard;
    [SerializeField] AbilityButton abilityPrefab;

    private GameState currentState;
    private MapPosition currentPosition;
    private Effect[] selectedAbility;
    private EquipmentCardSO selectedEquipment;
    private bool isPaused;
    private List<CharacterCardSO> characters = new List<CharacterCardSO>();
    private bool learningActivated;

    private void Start()
    {
        ResetAllCanvases();
        MapPosition.OnPositionSelected += SelectPosition;
        AbilityButton.OnAbilitySelected += SelectAbility;
        UpdatePositions(initialPosition);
        ChangeToState(GameState.Map);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Pause(!isPaused);
        }
    }

    public void Pause(bool pause)
    {
        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);
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
        newCharacterCanvas.SetActive(false);
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
            case GameState.Learning:
                ActivateCanvas(true, learningCanvas);
                //Learning Start Actions
                break;
            case GameState.NewCharacter:
                ActivateCanvas(true, newCharacterCanvas);
                GetNewCharacter();
                break;
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
                selectedAbility = null;
                ActivateCanvas(false, abilitiesCanvas);
                break;
            case GameState.Shop:
                selectedEquipment = null;
                ActivateCanvas(false, shopCanvas);
                break;
            case GameState.Learning:
                ActivateCanvas(false, learningCanvas);
                break;
            case GameState.NewCharacter:
                ActivateCanvas(false, newCharacterCanvas);
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
                if (currentPosition.IsShop)
                    ChangeToState(GameState.Shop);
                else if (learningActivated)
                    ChangeToState(GameState.Learning);
                else if (currentPosition.HasCharacter())
                    ChangeToState(GameState.NewCharacter);
                else
                    ChangeToState(GameState.Map);
                break;
            case GameState.Shop:
                if (learningActivated)
                    ChangeToState(GameState.Learning);
                else if (currentPosition.HasCharacter())
                    ChangeToState(GameState.NewCharacter);
                else
                    ChangeToState(GameState.Map);
                break;
            case GameState.Learning:
                if (currentPosition.HasCharacter())
                    ChangeToState(GameState.NewCharacter);
                else
                    ChangeToState(GameState.Map);
                break;
            case GameState.NewCharacter:
                ChangeToState(GameState.Map);
                break;
        }
    }

    private IEnumerator ApplyEffectsAndChangeState(CharacterCard selectedCharacter)
    {
        //Effect[] effectsToApply = eventCard.GetEffects();
        Effect[] effectsToApply = eventCard.GetEventOutcomeEffects(selectedCharacter);
        foreach (Effect effect in effectsToApply)
        {
            Debug.Log("Efecte: " + effect.affectedStat + " " + effect.affectionAmount);
            selectedCharacter.ApplyEffect(effect);
        }

        yield return new WaitForSeconds(1);

        ChangeToState(GameState.Abilities);
    }

    public void SelectAbility(Ability ability)
    {
        if (currentState != GameState.Abilities)
            return;

        //CharacterCard selectedCharacter = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInParent<CharacterCard>();
        selectedAbility = ability.GetEffects();
    }

    public void ApplySelectedAbility()
    {
        if (currentState != GameState.Abilities)
            return;

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
        if (currentState != GameState.Shop)
            return;

        EquipmentCard equipment = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInParent<EquipmentCard>();
        selectedEquipment = equipment.GetCardSO();
    }

    public void AssignEquipment()
    {
        if (currentState != GameState.Shop || selectedEquipment == null)
            return;

        CharacterCard selectedCharacter = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<CharacterCard>();
        selectedCharacter.AddEquipmentCard(selectedEquipment);
    }

    private void GetNewCharacter()
    {
        if (currentState != GameState.NewCharacter)
            return;

        newCharacterCard.AssignCard(currentPosition.GetCharacter());

        AddCharacter(currentPosition.GetCharacter());
    }

    public void AddCharacter(CharacterCardSO newCharacter)
    {
        characters.Add(newCharacter);
        foreach(CharacterLayoutController layout in characterCardsLayouts)
        {
            CharacterCard newCard = Instantiate(characterCardPrefab, layout.transform);
            if(layout.NeedsRearranging)
                layout.RearrangeLayout(newCard.transform);
            newCard.AssignCard(newCharacter);
        }
    }

    public void AddAbility(Transform abilitiesTransform, Ability abilityToAssign)
    {
        AbilityButton newAbility = Instantiate(abilityPrefab, abilitiesTransform);
        newAbility.AssignAbility(abilityToAssign);
    }

    private void OnDestroy()
    {
        MapPosition.OnPositionSelected -= SelectPosition;
    }
}

enum GameState
{
    Map, Event, Abilities, Shop, NewCharacter, Learning
}
