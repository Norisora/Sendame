using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckUserBaseTEST : MonoBehaviour
{
    [SerializeField]
    bool isPlayer;
    [SerializeField]
    CardObject cardPrefab;

    DeckDataTest deck;
    CardObject[] handCards;

    public int Life { get; private set; }
    public int ChargeCount { get; private set; }
    public CardObject SelectCardObject { get; private set; }
    protected bool IsTurnEnd { get; private set; }

    private void Awake()
    {
        handCards = new CardObject[5];
    }

    public virtual void TrunStart()
    {
        foreach (var v in handCards)
        {
            if (v != null)
            {
                v.ApplyCard(ChargeCount);
            }
        }
    }

    public IEnumerator Turn()
    {
        IsTurnEnd = false;
        
        while (!IsTurnEnd)
        {
            yield return null;
        }
    }

    public void DrawCard(int count)
    {
        while (count > 0)
        {
            var card = deck.DrawCard();

            for (int i = 0; i < handCards.Length; ++i)
            {
                if (handCards[i] == null)
                {
                    handCards[i] = Instantiate(cardPrefab);
                    if (isPlayer)
                    {
                        handCards[i].InitCard(card, SelectCard);
                    }
                    else
                    {
                        handCards[i].InitCard(card, null);
                    }
                    handCards[i].ApplyCard(ChargeCount);
                    //````
                    break;
                }
            }
            --count;
        }
    }
    public void SelectCard(CardObject cardObject)
    {
        SelectCardObject = cardObject;
        IsTurnEnd = true;
    }
    public void AddCharge(int chargeCount)
    {
        ChargeCount += chargeCount;
    }
}
