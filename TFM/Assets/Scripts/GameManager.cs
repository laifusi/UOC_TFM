using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] GameObject mapCanvas;
    [SerializeField] GameObject eventCanvas;
    [SerializeField] GameObject abilitiesCanvas;
    [SerializeField] GameObject shopCanvas;
    [SerializeField] GameObject learningCanvas;
    [SerializeField] GameObject newCharacterCanvas;
    [SerializeField] GameObject storyPointCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject winCanvas;
    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] GameObject miniMapCanvas;
    [Header("Map")]
    [SerializeField] MapPosition[] mapPositions;
    [SerializeField] MapPosition initialPosition;
    [SerializeField] CameraControl cameraControl;
    [Header("Event")]
    [SerializeField] EventCard eventCard;
    [Header("Shop")]
    [SerializeField] EquipmentCard[] equipmentCards;
    [SerializeField] CoinManager coinsManager;
    [Header("Characters")]
    [SerializeField] CharacterLayoutController[] characterCardsLayouts;
    [SerializeField] CharacterCard characterCardPrefab;
    [SerializeField] CharacterCard newCharacterCard;
    [SerializeField] CharacterCardSO startingCharacter;
    [SerializeField] OutcomeTextUI outcomeTextUI;
    [Header("Abilities")]
    [SerializeField] AbilityButton abilityPrefab;
    [SerializeField] TMP_Text abilitiesLeftText;
    [SerializeField] Transform learnableAbilitiesHolder;
    [SerializeField] LearnableAbilityUI learnableAbilityPrefab;
    [SerializeField] GameObject noAbilitiesAvailableText;
    //[SerializeField] int maxAbilitiesPerTurn;

    [Header("Sound Effects")]
    [SerializeField] AudioClip stepClip;
    [SerializeField] AudioClip shopClip;
    [SerializeField] AudioClip selectClip;
    [SerializeField] AudioClip freezeClip;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip loseClip;

    private GameState currentState;
    private MapPosition currentPosition;
    private Effect[] selectedAbility;
    private EquipmentCard selectedEquipment;
    private bool isPaused;
    private List<CharacterCardSO> characters = new List<CharacterCardSO>();
    private bool learningActivated;
    private StoryPoint currentStoryPoint;
    private int abilitiesUsedThisTurn;
    private bool tutorialsActive;
    private DeathTutorialTrigger deathTutorialTrigger;
    private AudioSource audioSource;

    private int maxAbilitiesPerTurn => characters.Count;

    public static Action<string, bool, bool> OnNewStoryLine;
    public static Action<bool> OnAbilitiesBlocked;
    public static Action OnStartNewTurn;
    public static Action<bool> OnCharacterClickableChange;

    public GameState CurrentState => currentState;
    public static GameManager Instance;
    public bool TutorialsActive => tutorialsActive;
    public MusicZone MusicZone => currentPosition.MusicZone;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        deathTutorialTrigger = GetComponent<DeathTutorialTrigger>();
        ResetAllCanvases();
        MapPosition.OnPositionSelected += SelectPosition;
        AbilityButton.OnAbilitySelected += SelectAbility;
        OptionsManager.OnLanguageChanged += UpdateLanguage;
        tutorialsActive = true;
        
        StartCoroutine(InitializeGameState());
    }

    private IEnumerator InitializeGameState()
    {
        yield return null;
        UpdatePositions(initialPosition);
        AddCharacter(startingCharacter);
        ChangeToState(GameState.StoryPoint);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Pause(!isPaused);
        }

        if(Input.GetKeyUp(KeyCode.M) && currentState == GameState.Map && !IsTutorialOn())
        {
            miniMapCanvas.SetActive(!miniMapCanvas.activeInHierarchy);
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
        newCharacterCanvas.SetActive(true);
        storyPointCanvas.SetActive(true);
        learningCanvas.SetActive(true);

        mapCanvas.SetActive(false);
        eventCanvas.SetActive(false);
        abilitiesCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        newCharacterCanvas.SetActive(false);
        storyPointCanvas.SetActive(false);
        learningCanvas.SetActive(false);
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
                if (CheckGameOver())
                    ChangeToState(GameState.GameOver);
                else
                {
                    OnStartNewTurn?.Invoke();
                    ActivateMap(true);
                    ActivateCanvas(true, mapCanvas);
                }
                break;
            case GameState.Event:
                ActivateCanvas(true, eventCanvas);
                OnCharacterClickableChange?.Invoke(true);
                GetRandomEvent();
                break;
            case GameState.Abilities:
                ActivateCanvas(true, abilitiesCanvas);
                abilitiesLeftText.SetText((maxAbilitiesPerTurn - abilitiesUsedThisTurn).ToString());
                OnCharacterClickableChange?.Invoke(false);
                break;
            case GameState.Shop:
                ActivateCanvas(true, shopCanvas);
                GetRandomShopCards();
                OnCharacterClickableChange?.Invoke(false);
                break;
            case GameState.Learning:
                ActivateCanvas(true, learningCanvas);
                ResetLearningCanvas();
                break;
            case GameState.NewCharacter:
                ActivateCanvas(true, newCharacterCanvas);
                GetNewCharacter();
                break;
            case GameState.StoryPoint:
                ActivateCanvas(true, storyPointCanvas);
                GetNextStoryLine();
                break;
            case GameState.GameOver:
                ActivateCanvas(true, gameOverCanvas);
                PlayClip(loseClip);
                break;
            case GameState.EndGame:
                ActivateCanvas(true, winCanvas);
                MusicManager.Instance.StopMusic();
                PlayClip(winClip);
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
                outcomeTextUI.UpdateOutcomeUI("", transform, false);
                deathTutorialTrigger.CheckForDeath(characters);
                break;
            case GameState.Abilities:
                selectedAbility = null;
                abilitiesUsedThisTurn = 0;
                ActivateCanvas(false, abilitiesCanvas);
                OnAbilitiesBlocked?.Invoke(false);
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
            case GameState.StoryPoint:
                ActivateCanvas(false, storyPointCanvas);
                if (currentStoryPoint.IsBook)
                    ActivateLearning();
                break;
        }
    }

    private void ActivateCanvas(bool activate, GameObject canvas)
    {
        canvas.SetActive(activate);
    }

    public bool IsTutorialOn()
    {
        return tutorialCanvas.activeInHierarchy;
    }

    public void ChangeStateWithButton()
    {
        if(IsTutorialOn())
        {
            return;
        }

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
                else if (InPostEventStoryPoint())
                    ChangeToState(GameState.StoryPoint);
                else
                    ChangeToState(GameState.Map);
                break;
            case GameState.Shop:
                if (learningActivated)
                    ChangeToState(GameState.Learning);
                else if (InPostEventStoryPoint())
                    ChangeToState(GameState.StoryPoint);
                else
                    ChangeToState(GameState.Map);
                break;
            case GameState.Learning:
                if (InPostEventStoryPoint())
                    ChangeToState(GameState.StoryPoint);
                else
                    ChangeToState(GameState.Map);
                break;
            case GameState.NewCharacter:
                ChangeToState(GameState.Map);
                break;
            case GameState.StoryPoint:
                if (currentStoryPoint.IsLastSP)
                    ChangeToState(GameState.EndGame);
                else if (InPreEventStoryPoint())
                    ChangeToState(GameState.Event);
                else if (InPostEventStoryPoint())
                {
                    if (currentPosition.HasCharacter())
                        ChangeToState(GameState.NewCharacter);
                    else
                        ChangeToState(GameState.Map);
                }
                currentStoryPoint.MarkPlayed();
                break;
        }
    }

    private void UpdateLanguage()
    {
        if(currentState == GameState.StoryPoint)
            OnNewStoryLine?.Invoke(currentStoryPoint.GetCurrentLine(), currentStoryPoint.IsLastLine(), currentStoryPoint.IsFirstLine());
    }

    private bool CheckGameOver()
    {
        foreach(CharacterCardSO character in characters)
        {
            if (!character.IsFrozen)
                return false;
        }
        return true;
    }

    private void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    #region Game State: Map
    private void ActivateMap(bool activate)
    {
        foreach (MapPosition pos in mapPositions)
        {
            pos.InMapState = activate;
        }
    }

    private void SelectPosition(MapPosition newMapPosition)
    {
        UpdatePositions(newMapPosition);
        cameraControl.MoveCamera(newMapPosition.transform);

        PlayClip(stepClip);

        if (InPreEventStoryPoint())
            ChangeToState(GameState.StoryPoint);
        else
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
        currentStoryPoint = currentPosition.GetStoryPoint();
    }
    #endregion

    #region Game State: Event
    private void GetRandomEvent()
    {
        EventCardSO cardEvent = currentPosition.GetEvent()[0];
        eventCard.PaintCard(cardEvent);
    }

    public void ActivateOutcomeInfo(CharacterCard character, bool activate)
    {
        string outcomeText = activate ? eventCard.GetPossibleOutcomeInfo(character) : "";

        outcomeTextUI.UpdateOutcomeUI(outcomeText, character.transform, activate);
    }

    private IEnumerator ApplyEffectsAndChangeState(CharacterCard selectedCharacter)
    {
        OnCharacterClickableChange?.Invoke(false);
        selectedCharacter.IncreaseUses();
        Effect[] effectsToApply = eventCard.GetEventOutcomeEffects(selectedCharacter);
        if(effectsToApply != null && effectsToApply.Length > 0)
        {
            foreach (Effect effect in effectsToApply)
            {
                Debug.Log("Efecte: " + effect.affectedStat + " " + effect.affectionAmount);
                selectedCharacter.ApplyEffect(effect);
            }
            yield return new WaitForSeconds(1f);
        }
        else
        {
            PlayClip(selectClip);
            yield return new WaitForSeconds(0.2f);
        }

        ChangeToState(GameState.Abilities);
    }
    #endregion

    #region GameState: Abilities
    public void SelectAbility(Ability ability)
    {
        if (currentState != GameState.Abilities || abilitiesUsedThisTurn >= maxAbilitiesPerTurn)
            return;

        selectedAbility = ability.GetEffects();
        OnCharacterClickableChange?.Invoke(true);
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
            abilitiesUsedThisTurn++;
            abilitiesLeftText.SetText((maxAbilitiesPerTurn - abilitiesUsedThisTurn).ToString());
            if (maxAbilitiesPerTurn - abilitiesUsedThisTurn <= 0)
                OnAbilitiesBlocked?.Invoke(true);
            selectedAbility = null;

            OnCharacterClickableChange?.Invoke(false);
        }
    }
    #endregion

    #region GameState: Shop
    private void GetRandomShopCards()
    {
        EquipmentCardSO[] shopCards = currentPosition.GetShop();
        for (int i = 0; i < shopCards.Length; i++)
        {
            equipmentCards[i].PaintCard(shopCards[i]);
            equipmentCards[i].ActivateSoldText(false);
            if(CoinManager.Instance.CanBeBought(shopCards[i]))
            {
                equipmentCards[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                equipmentCards[i].GetComponent<Button>().interactable = false;
            }
        }

        PlayClip(shopClip);
    }

    public void SelectEquipment()
    {
        if (currentState != GameState.Shop)
            return;

        selectedEquipment = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInParent<EquipmentCard>();
        OnCharacterClickableChange?.Invoke(true);

        PlayClip(selectClip);
    }

    public void AssignEquipment()
    {
        if (currentState != GameState.Shop || selectedEquipment == null)
            return;

        CharacterCard selectedCharacter = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<CharacterCard>();
        selectedCharacter.AddEquipmentCard(selectedEquipment.GetCardSO());
        selectedEquipment.GetComponent<Button>().interactable = false;
        selectedEquipment.ActivateSoldText(true);
        foreach(EquipmentCard equipment in equipmentCards)
        {
            Button equipmentButton = equipment.GetComponent<Button>();
            if(equipmentButton.interactable && !CoinManager.Instance.CanBeBought(equipment.GetCardSO()))
            {
                equipmentButton.interactable = false;
            }
        }
        selectedEquipment = null;
        OnCharacterClickableChange?.Invoke(false);
    }
    #endregion

    #region GameState: Learning
    [ContextMenu("ActivateLearning")]
    public void ActivateLearning()
    {
        learningActivated = true;
    }

    private void ResetLearningCanvas()
    {
        foreach(Transform child in learnableAbilitiesHolder)
        {
            Destroy(child.gameObject);
        }
        noAbilitiesAvailableText.SetActive(false);
    }

    public void ShowLearnableAbilities(CharacterCard character)
    {
        ResetLearningCanvas();
        List<Ability> learnableAbilities = character.GetLearnableAbilities();
        foreach(Ability learnableAbility in learnableAbilities)
        {
            LearnableAbilityUI abilityUI = Instantiate(learnableAbilityPrefab, learnableAbilitiesHolder);
            abilityUI.AssignAbility(learnableAbility, character);
        }

        noAbilitiesAvailableText.SetActive(learnableAbilities.Count == 0);
    }

    public void AddAbility(Transform abilitiesTransform, Ability abilityToAssign)
    {
        AbilityButton newAbility = Instantiate(abilityPrefab, abilitiesTransform);
        newAbility.AssignAbility(abilityToAssign);
    }
    #endregion

    #region StoryPoint
    public bool InPreEventStoryPoint()
    {
        return currentStoryPoint != null && currentStoryPoint.IsPreEvent && currentStoryPoint.ShouldPlay && IsNotRestrictedByCharacters();
    }

    public bool InPostEventStoryPoint()
    {
        return currentStoryPoint != null && !currentStoryPoint.IsPreEvent && currentStoryPoint.ShouldPlay && IsNotRestrictedByCharacters();
    }

    private bool IsNotRestrictedByCharacters()
    {
        return (currentPosition.HasCharacter() && characters.Count < 4) || !currentPosition.HasCharacter();
    }

    public void GetNextStoryLine()
    {
        string nextLine = currentStoryPoint.GetNextLine();
        bool isLastLine = currentStoryPoint.IsLastLine();
        bool isFirstLine = currentStoryPoint.IsFirstLine();
        OnNewStoryLine?.Invoke(nextLine, isLastLine, isFirstLine);
    }

    public void GetPreviousStoryLine()
    {
        string previousLine = currentStoryPoint.GetPreviousLine();
        bool isLastLine = currentStoryPoint.IsLastLine();
        bool isFirstLine = currentStoryPoint.IsFirstLine();
        OnNewStoryLine?.Invoke(previousLine, isLastLine, isFirstLine);
    }

    #region GameState: New Character
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
        newCharacter.OnCharacterFrozen += PlayFreezeClip;

        foreach (CharacterLayoutController layout in characterCardsLayouts)
        {
            CharacterCard newCard = Instantiate(characterCardPrefab, layout.transform);
            if (layout.NeedsRearranging)
                layout.RearrangeLayout(newCard.transform);
            newCard.AssignCard(newCharacter);
            newCard.ActivateButtons(layout.State, this);
            foreach (Ability ability in newCard.GetActiveAbilities())
            {
                AddAbility(newCard.GetAbilitiesParent(), ability);
            }
        }

        coinsManager.AddCharacter(newCharacter);
    }

    private void PlayFreezeClip(bool freeze)
    {
        PlayClip(freezeClip);
    }
    #endregion

    #endregion

    private void OnDestroy()
    {
        MapPosition.OnPositionSelected -= SelectPosition;
        AbilityButton.OnAbilitySelected -= SelectAbility;
        OptionsManager.OnLanguageChanged -= UpdateLanguage;
        foreach(CharacterCardSO character in characters)
        {
            character.OnCharacterFrozen -= PlayFreezeClip;
        }
    }
}

public enum GameState
{
    Map, Event, Abilities, Shop, NewCharacter, Learning, StoryPoint, GameOver, EndGame
}
