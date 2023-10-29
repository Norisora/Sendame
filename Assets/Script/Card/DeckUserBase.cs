using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckUserBase : MonoBehaviour
{
    [SerializeField]
    bool isPlayer;
    [SerializeField]
    CardController cardPrefab;

    [SerializeField]
    TextMeshProUGUI UIText;

    public DeckData deckData;
    protected CardController[] handCards;

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
        Life = 3;
        ApplyUI();
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

    public virtual IEnumerator Turn()
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
            for (int i = 0; i < handCards.Length; i++)
            {
                if (handCards[i] == null)
                {
                    Vector2 initPos = InitPosCalc(parent, i);   //InitPosCalcで配置ポジションの計算している
                    handCards[i] = Instantiate(cardPrefab, initPos, Quaternion.identity, parent);
                    if (isPlayer)
                    {
                        handCards[i].InitCard(cardData, SelectCard);
                        Debug.Log("プレイヤーInitCard時のID" + cardData.CardModel.cardID);
                    }
                    else
                    {
                        handCards[i].InitCard(cardData, null);
                        Debug.Log("相手のInitCard時のID" + cardData.CardModel.cardID);
                    }
                    handCards[i].ApplyCard(ChargeCount);
                    break;
                }
            }
            count--;
        }
    }

    public void SelectCard(CardController selected)
    {
        SelectCardObject = selected;
        Debug.Log("selectカードタイプ" + SelectCardObject.Data.CardModel.cardType);     //カードタイプはAttack
        Debug.Log("selectカードID" + SelectCardObject.Data.CardModel.cardID);           //IDはクリックしたカードのID
        for (int i = 0; i < handCards.Length; ++i)
        {
            if (handCards[i] == selected)
            {
                handCards[i] = null;    //カード出した
            }
        }

        IsTurnEnd = true;
        Destroy(SelectCardObject.gameObject);   //わかりやすくするため
    }
    public void Charge(int chargeCount)
    {
        ChargeCount += chargeCount;
        ApplyUI();
        Debug.Log("チャージ" +  ChargeCount);
    }

    public void GetDamage(int cardAttackPoint)
    {
        Life -= cardAttackPoint;
        ApplyUI() ;
    }
    //カードドロー時の手札内の配置ポジション計算
    Vector2 InitPosCalc(Transform parent, int i)
    {
        float posX = parent.position.x - firstCardPos + (i * cardSpacing);

        return new Vector2(posX, parent.position.y);
    }
    void ApplyUI()
    {
        UIText.text = $"{Life} {ChargeCount}";
    }
}
