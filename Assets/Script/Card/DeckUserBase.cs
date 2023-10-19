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
    public int ChargeCount { get; private set; }
    public CardController SelectCardObject { get; private set; }
    protected bool IsTurnEnd { get; private set; }

    float cardSpacing = 5.0f;   //手札内のカードの間隔
    float firstCardPos = 9.0f;
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

    public void DrawCard(int count, Transform parent)
    {
        while (count > 0)
        {
            var cardData = deckData.PassCard();     //デッキの一番上のCardDataを渡す
            Debug.Log("for直前のカードID" + cardData.ID);
            for (int i = 0; i < handCards.Length; i++)
            {
                Debug.Log("for直後のカードID" + cardData.ID);
                if (handCards[i] == null)
                {
                    Vector2 initPos = InitPosCalc(parent, i);   //InitPosCalcで配置ポジションの計算している
                    handCards[i] = Instantiate(cardPrefab, initPos, Quaternion.identity, parent);
                    if (isPlayer)
                    {
                        handCards[i].InitCard(cardData, SelectCard);
                        Debug.Log("プレイヤーInitCard時のID" + cardData.ID);
                    }
                    else
                    {
                        handCards[i].InitCard(cardData, null);
                        Debug.Log("相手のInitCard時のID" + cardData.ID);
                    }
                    handCards[i].ApplyCard(ChargeCount);
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
        ChargeCount += buildUpCount;
    }

    //カードドロー時の手札内の配置ポジション計算
    Vector2 InitPosCalc(Transform parent, int i)
    {
        float posX = parent.position.x - firstCardPos + (i * cardSpacing);

        return new Vector2(posX, parent.position.y);
    }
}
