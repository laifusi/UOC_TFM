using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class CharacterCard : Card<CharacterCardSO>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image characterImage;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text attackText;
    [SerializeField] TMP_Text shieldText;
    [SerializeField] TMP_Text intelligenceText;
    [SerializeField] TMP_Text socialSkillsText;
    [SerializeField] TMP_Text agilityText;
    [SerializeField] TMP_Text dexterityText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image powerImage;
    [SerializeField] Transform equipmentHolder;
    [SerializeField] EquipmentCard equipmentCardPrefab;
    [SerializeField] Button button;
    [SerializeField] Transform abilitiesHolder;
    [SerializeField] int usesBeforeLevelIncrease = 3;
    [SerializeField] TMP_Text frozenText;
    [SerializeField] LocalizedString deadText;
    [SerializeField] LocalizedString learningText;

    [Header("PopUp Effect Animation")]
    [SerializeField] TMP_Text popUpAnimation;
    [SerializeField] Transform agilityPopUpPos;
    [SerializeField] Transform attackPopUpPos;
    [SerializeField] Transform dexterityPopUpPos;
    [SerializeField] Transform healthPopUpPos;
    [SerializeField] Transform intelligencePopUpPos;
    [SerializeField] Transform shieldPopUpPos;
    [SerializeField] Transform socialSkillsPopUpPos;
    [SerializeField] Vector3 popUpOffset;

    private GameState cardsAssignedState;
    private int usesSinceLastLevelIncrease;
    private int turnsLearning;
    private int turnsToLearn;
    private Animator animator;
    private Transform popUpParent;
    private Color originalCardColor;

    public Action OnStartedLearning;

    private void Start()
    {
        originalCardColor = cardImage.color;
        popUpParent = transform;
        animator = GetComponent<Animator>();

        card.OnCharacterFrozen += FreezeCharacter;
        card.OnAbilityLearnt += UpdateAbilities;
        OptionsManager.OnLanguageChanged += ChangeFrozenText;
        GameManager.OnAbilitiesBlocked += BlockAbilityButtons;
        GameManager.OnCharacterClickableChange += ClickableButton;
        if (cardsAssignedState == GameState.Learning)
            GameManager.OnStartNewTurn += UpdateLearningState;
        if (cardsAssignedState == GameState.Abilities || cardsAssignedState == GameState.Shop)
            ClickableButton(false);
        else
            ClickableButton(true);

        if (card.IsFrozen)
            FreezeCharacter(true);
        else
            FreezeCharacter(false);
    }

    public override void PaintCard(CharacterCardSO cardToPaint)
    {
        card = cardToPaint;
        card.ResetCard();
        UpdateUI();
        UpdateEquipmentUI();

        if (card.GetCoins() > 0)
        {
            ShowEffectPopUp(StatType.Coins, card.GetCoins());
        }
    }

    private void UpdateUI()
    {
        nameText.text = card.GetName();
        healthText.text = card.GetHealth().ToString();
        shieldText.text = card.GetShield().ToString();
        attackText.text = card.GetAttack().ToString();
        intelligenceText.text = card.GetStat(StatType.Intelligence).ToString();
        socialSkillsText.text = card.GetStat(StatType.SocialSkills).ToString();
        agilityText.text = card.GetStat(StatType.Agility).ToString();
        dexterityText.text = card.GetStat(StatType.Dexterity).ToString();
        powerImage.sprite = IconManager.Instance.GetPowerSprite(card.GetPower());
    }

    // Temporary fix to update equipment cards on character cards
    private void UpdateEquipmentUI()
    {
        foreach (Transform child in equipmentHolder)
        {
            Destroy(child.gameObject);
        }

        List<EquipmentCardSO> equipmentCards = card.GetEquipment();
        foreach (EquipmentCardSO equipment in equipmentCards)
        {
            EquipCard(equipment);
        }
    }

    private void EquipCard(EquipmentCardSO equipment)
    {
        EquipmentCard equipCard = Instantiate(equipmentCardPrefab, equipmentHolder);
        equipCard.PaintCard(equipment);
    }

    public void ApplyEffect(Effect effect)
    {
        card.ApplyEffect(effect);
        ShowEffectPopUp(effect.affectedStat, effect.affectionAmount);
        UpdateUI();
    }

    private void ShowEffectPopUp(StatType stat, float amount)
    {
        Vector3 popUpPosition = Vector3.zero;
        switch (stat)
        {
            case StatType.Agility:
                popUpPosition = agilityPopUpPos.position;
                break;
            case StatType.Attack:
                popUpPosition = attackPopUpPos.position;
                break;
            case StatType.Coins:
                popUpParent = CoinManager.Instance.GetPopUpTransform();
                popUpPosition = popUpParent.position;
                break;
            case StatType.Dexterity:
                popUpPosition = dexterityPopUpPos.position;
                break;
            case StatType.Health:
                popUpPosition = healthPopUpPos.position;
                break;
            case StatType.Intelligence:
                popUpPosition = intelligencePopUpPos.position;
                break;
            case StatType.Shield:
                popUpPosition = shieldPopUpPos.position;
                break;
            case StatType.SocialSkills:
                popUpPosition = socialSkillsPopUpPos.position;
                break;
        }
        TMP_Text popUp = Instantiate(popUpAnimation, popUpPosition + popUpOffset, Quaternion.identity, popUpParent);
        if (amount > 0)
            popUp.SetText("+" + amount.ToString());
        else
            popUp.SetText(amount.ToString());
        Destroy(popUp.gameObject, 1f);
    }

    public Effect[] GetEffects()
    {
        return card.GetEffects();
    }

    public void AddEquipmentCard(EquipmentCardSO equipment)
    {
        bool isBought = CoinManager.Instance.BuyCard(equipment);
        if (isBought)
        {
            card.AddEquipment(equipment);
            EquipCard(equipment);

            Effect[] effects = equipment.GetEffects();
            foreach (Effect effect in effects)
            {
                ApplyEffect(effect);
            }

            ShowEffectPopUp(StatType.Coins, -equipment.GetCost());
        }
    }

    public int GetCoins()
    {
        return card.GetCoins();
    }

    public Power GetPower()
    {
        return card.GetPower();
    }

    public int GetStat(StatType statType)
    {
        return card.GetStat(statType);
    }

    public bool IsFullAttack(float enemyLife)
    {
        return card.IsFullAttack(enemyLife);
    }

    public void GetAttacked(float attack)
    {
        ShowAttackPopUp(attack);
        card.GetAttacked(attack);
    }

    private void ShowAttackPopUp(float attack)
    {
        float shield = card.GetShield();
        float shieldAmount = 0;
        float healthAmount = 0;
        if (shield > 0)
        {
            if (attack <= shield)
            {
                shieldAmount = attack;
            }
            else
            {
                shieldAmount = shield;
                healthAmount = attack - shield;
            }
        }
        else
        {
            healthAmount = attack;
        }

        if(shieldAmount != 0)
            ShowEffectPopUp(StatType.Shield, -shieldAmount);

        if(healthAmount != 0)
            ShowEffectPopUp(StatType.Health, -healthAmount);
    }

    public void ActivateButtons(GameState state, GameManager manager)
    {
        cardsAssignedState = state;

        switch (state)
        {
            case GameState.Event:
                button.enabled = true;
                button.onClick.AddListener(manager.ChangeStateWithButton);
                break;
            case GameState.Abilities:
                button.enabled = true;
                button.onClick.AddListener(manager.ApplySelectedAbility);
                break;
            case GameState.Shop:
                button.enabled = true;
                button.onClick.AddListener(manager.AssignEquipment);
                break;
            case GameState.Learning:
                button.enabled = true;
                button.onClick.AddListener(() => { manager.ShowLearnableAbilities(this); });
                break;
        }
    }

    public void FreezeCharacter(bool isFrozen)
    {
        BlockAbilityButtons(isFrozen);
        switch (cardsAssignedState)
        {
            case GameState.Event:
                button.interactable = !isFrozen;
                break;
            /*case GameState.Abilities:
                BlockAbilityButtons(isFrozen);
                break;*/
            case GameState.Learning:
                button.interactable = !isFrozen;
                break;
        }

        cardImage.color = card.IsFrozen ? Color.gray : originalCardColor;
        ChangeFrozenText();
    }

    private void BlockAbilityButtons(bool isBlocked)
    {
        foreach (Transform child in abilitiesHolder)
            child.GetComponent<Button>().interactable = !isBlocked && !card.IsFrozen;
    }

    public List<Ability> GetActiveAbilities()
    {
        return card.GetActiveAbilities();
    }

    public List<Ability> GetLearnableAbilities()
    {
        return card.GetLearnableAbilities();
    }

    public Transform GetAbilitiesParent()
    {
        return abilitiesHolder;
    }

    public void LearnAbility(Ability ability)
    {
        card.LearnAbility(ability);
        turnsToLearn = ability.abilityCost;
        turnsLearning = 0;
        OnStartedLearning?.Invoke();
    }

    public void UpdateLearningState()
    {
        if (card.IsLearning && turnsLearning >= turnsToLearn)
            card.FinishLearning();
        turnsLearning++;
    }

    public void IncreaseUses()
    {
        usesSinceLastLevelIncrease++;
        if(usesSinceLastLevelIncrease >= usesBeforeLevelIncrease)
        {
            usesSinceLastLevelIncrease = 0;
            card.IncreaseAbilityLevel();
        }
    }

    private void UpdateAbilities(Ability newAbility)
    {
        GameManager.Instance.AddAbility(abilitiesHolder, newAbility);
    }

    private void ClickableButton(bool clickable)
    {
        animator.SetBool("shouldHighlight", clickable);
    }

    private void ChangeFrozenText()
    {
        if (card.IsFrozen)
        {
            if (card.IsLearning)
            {
                frozenText.SetText(learningText.GetLocalizedString());
            }
            else
            {
                frozenText.SetText(deadText.GetLocalizedString());
            }
        }
        else
        {
            frozenText.SetText("");
        }
    }

    private void OnEnable()
    {
        if(card != null)
        {
            UpdateUI();
            UpdateEquipmentUI();
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
        if(card != null)
        {
            card.OnCharacterFrozen -= FreezeCharacter;
            card.OnAbilityLearnt -= UpdateAbilities;
        }

        OptionsManager.OnLanguageChanged -= ChangeFrozenText;
        GameManager.OnAbilitiesBlocked -= BlockAbilityButtons;
        GameManager.OnCharacterClickableChange -= ClickableButton;
        if (cardsAssignedState == GameState.Learning)
            GameManager.OnStartNewTurn -= UpdateLearningState;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentState == GameState.Event && !card.IsFrozen)
            GameManager.Instance.ActivateOutcomeInfo(this, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentState == GameState.Event)
            GameManager.Instance.ActivateOutcomeInfo(this, false);
    }
}
