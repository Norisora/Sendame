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
        deckData = new DeckData();
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
            var cardData = deckData.PassCard();     //デッキの一番上のCardDataを渡す
            Debug.Log("for直前のカードID" + cardData.ID);      //ID=1が5回、ID=2が５回、ID=3が５回
            for (int i = 0; i < handCards.Length; i++)
            {
                Debug.Log("for直後のカードID" + cardData.ID);      //ID=1が5回、ID=2が５回、ID=3が５回
                if (handCards[i] == null)
                {
                    handCards[i] = Instantiate(cardPrefab);
                    if (isPlayer)
                    {
                        handCards[i].InitCard(cardData, SelectCard);
                        Debug.Log("プレイヤーInitCard時のID" + cardData.ID);    //ID=1が５回。その後処理なし
                    }
                    else
                    {
                        handCards[i].InitCard(cardData, null);
                        Debug.Log("相手のInitCard時のID" + cardData.ID);     //ID=1が５回
                    }
                    handCards[i].ApplyCard(BuildUpCount);
                    break;
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
