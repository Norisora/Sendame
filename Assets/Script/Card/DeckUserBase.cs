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
    TextMeshProUGUI LifeValueText, ChargeValueText, DeckCountValue;

    [SerializeField]
    NumberAnimationGenerator lifeUI;
    [SerializeField]
    NumberAnimationGenerator chargeUI;

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
        Life = 100;
        ApplyUI();
    }
    public CardController[] GetHandCards()
    {
        return handCards;
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
            DeckCountValue.text = $"{deckData.GetDeckCount()}";  //デッキカウントUI更新
            if (cardData == null)
            {
                Debug.Log("山札がない");
                Life = 0;
                break;
            }
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
        if (selected == null) return;
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
    }
    public void Charge(int chargeCount)
    {
        if (chargeCount < 0)
        {
            chargeUI.GenerateNumber(chargeCount, Color.black);
        }
        else
        {
            chargeUI.GenerateNumber(chargeCount, Color.green);
        }
        ChargeCount += chargeCount;
        ApplyUI();
    }

    public void GetDamage(int damagePoint)
    {
        //var LifeRT = LifeValueText.rectTransform.gameObject;
        lifeUI.GenerateNumber(damagePoint, Color.red);
        Life -= damagePoint;    //MathF絶対値にしたほうがいい？
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
        LifeValueText.text = $"{Life}";
        ChargeValueText.text = $"{ChargeCount}";
    }

    public void MoveToField(CardController selectedCard,Transform parent)
    {
        if (selectedCard == null)
        {
            return;
        }
        selectedCard.transform.SetParent(parent);
        selectedCard.transform.localPosition = Vector2.zero;
        Destroy(selectedCard.gameObject, 1.0f);
    }

}
