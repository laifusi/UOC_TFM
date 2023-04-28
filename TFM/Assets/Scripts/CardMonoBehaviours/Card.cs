using UnityEngine;
using UnityEngine.UI;

public abstract class Card <T> : MonoBehaviour
{
    [SerializeField] protected T baseCard;
    [SerializeField] protected Image cardImage;

    protected T card;

    public void AssignCard(T cardToAssign)
    {
        card = cardToAssign;
        PaintCard(card);
    }

    public abstract void PaintCard(T cardToPaint);

    public T GetCardSO()
    {
        return card;
    }
}
