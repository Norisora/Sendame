using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckUserBase : MonoBehaviour
{
    [SerializeField]
    bool isPlayer;
    [SerializeField]
    CardController cardPrefab;

    public DeckData deckData;
    CardController[] handCards;

    public int Life { get; private set; }
    public int BuildUpCount { get; private set; }
    public CardController SelectCardObject { get; private set; }
    protected bool IsTurnEnd { get; private set; }

    private void Awake()
    {
        handCards = new CardController[5];
        //deckData = new DeckData();
    }

    public virtual void TurnStart()
    {
        foreach (var v in handCards)
        {
            if (v != null)
            {
                v.ApplyCard(BuildUpCount);
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
            var cardData = deckData.PassCard();     //デッキの一番上のCardData1を渡す
            
            for (int i = 0; i < handCards.Length; i++)
            {
                if (handCards[i] == null)
                {
                    handCards[i] = Instantiate(cardPrefab);
                    if (isPlayer)
                    {
                        handCards[i].InitCard(cardData, SelectCard);
                        Debug.Log("カードID" + cardData.ID);
                    }
                    else
                    {
                        handCards[i].InitCard(cardData, null);
                    }
                    handCards[i].ApplyCard(BuildUpCount);
                }
            }
            count--;
        }
    }

    public void SelectCard(CardController cardController)
    {
        SelectCardObject = cardController;
        IsTurnEnd = true;
    }
    public void BuildUp(int buildUpCount)
    {
        BuildUpCount += buildUpCount;
    }
}
