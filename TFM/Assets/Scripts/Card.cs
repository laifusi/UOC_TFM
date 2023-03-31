using UnityEngine;
using UnityEngine.UI;

public abstract class Card <T> : MonoBehaviour
{
    [SerializeField] protected T baseCard;
    [SerializeField] protected Image cardImage;

    protected T card;

    public abstract void PaintCard(T cardToPaint);
}
