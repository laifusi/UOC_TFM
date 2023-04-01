using System;
using System.Collections.Generic;

[Serializable]
public class Deck<T> where T : CardSO
{
    public T[] possibleCards;
    public int cardsToDraw;

    private List<T> commonCards = new List<T>();
    private List<T> uncommonCards = new List<T>();
    private List<T> rareCards = new List<T>();

    public T[] Draw()
    {
        T[] cardsDrawn = new T[cardsToDraw];

        //We extract the cards from each rarity pool
        foreach (T card in possibleCards)
        {
            switch(card.rarity)
            {
                case Rarity.Common:
                    commonCards.Add(card);
                    break;
                case Rarity.Uncommon:
                    uncommonCards.Add(card);
                    break;
                case Rarity.Rare:
                    rareCards.Add(card);
                    break;
            }
        }

        for(int i = 0; i < cardsToDraw; i++)
        {
            // We choose a random value to select the rarity pool
            // Rare = 20%
            // Uncommon = 30%
            // Common = 50%
            List<T> selectedPoolCards;
            float rarityValue = UnityEngine.Random.Range(0f, 1f);
            if (rarityValue < 0.2f)
            {
                selectedPoolCards = rareCards;
            }
            else if(rarityValue < 0.5f)
            {
                selectedPoolCards = uncommonCards;
            }
            else
            {
                selectedPoolCards = commonCards;
            }

            // We get a random card from the randomly selected pool
            cardsDrawn[i] = selectedPoolCards[UnityEngine.Random.Range(0, selectedPoolCards.Count)];
        }

        return cardsDrawn;
    }
}
