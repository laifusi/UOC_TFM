using UnityEngine;

public class MapPosition : MonoBehaviour
{
    [SerializeField] private MapPosition[] connectedPositions;
    [SerializeField] private Deck eventDeck;

    [Header("Shop")]
    [SerializeField] private bool isShop;
    [SerializeField] private Deck shopDeck;

    private bool isAccessible;
    private bool isCurrent;


}
