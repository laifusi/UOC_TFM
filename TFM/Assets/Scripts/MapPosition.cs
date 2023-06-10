using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapPosition : MonoBehaviour
{
    [SerializeField] private MapPosition[] connectedPositions;
    [SerializeField] private Deck<EventCardSO> eventDeck;

    [Header("Position colors")]
    [SerializeField] private Color redColor;
    [SerializeField] private Color yellowColor;
    [SerializeField] private Color blueColor;

    [Header("Shop")]
    [SerializeField] private bool isShop;
    [SerializeField] private Deck<EquipmentCardSO> shopDeck;

    [Header("Other position configuration")]
    [SerializeField] private bool isConnected;
    [SerializeField] private bool isCurrent;
    [SerializeField] private bool inMapState;
    [SerializeField] private CharacterCardSO foundCharacter;

    private SpriteRenderer positionSprite;
    private StoryPoint storyPoint;
    private bool characterAdded;
    private Animator animator;

    public bool IsConnected { get => isConnected; set => isConnected = value; }
    public bool IsCurrent { get => isCurrent; set => isCurrent = value; }
    public bool IsShop => isShop;
    public bool InMapState { get => inMapState; set => inMapState = value; }

    public static Action<MapPosition> OnPositionSelected;

    private void Awake()
    {
        positionSprite = GetComponent<SpriteRenderer>();
        storyPoint = GetComponent<StoryPoint>();
        animator = GetComponent<Animator>();
    }

    public void UpdatePositionColor()
    {
        if(isCurrent)
        {
            positionSprite.color = blueColor;
        }
        else if(isConnected)
        {
            positionSprite.color = yellowColor;
        }
        else
        {
            positionSprite.color = redColor;
        }
    }

    private void OnMouseEnter()
    {
        if(IsConnected && InMapState && !GameManager.Instance.IsTutorialOn())
            animator.SetBool("mouseOver", true);
    }

    private void OnMouseExit()
    {
        animator.SetBool("mouseOver", false);
    }

    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

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

    public StoryPoint GetStoryPoint()
    {
        return storyPoint;
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
        return foundCharacter != null && !characterAdded;
    }

    public CharacterCardSO GetCharacter()
    {
        characterAdded = true;
        return foundCharacter;
    }
}
