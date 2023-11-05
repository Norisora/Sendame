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

    float cardSpacing = 5.0f;   //��D���̃J�[�h�̊Ԋu
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
            var cardData = deckData.PassCard();     //�f�b�L�̈�ԏ��CardData��n��
            DeckCountValue.text = $"{deckData.GetDeckCount()}";  //�f�b�L�J�E���gUI�X�V
            if (cardData == null)
            {
                Debug.Log("�R�D���Ȃ�");
                Life = 0;
                break;
            }
            for (int i = 0; i < handCards.Length; i++)
            {
                if (handCards[i] == null)
                {
                    Vector2 initPos = InitPosCalc(parent, i);   //InitPosCalc�Ŕz�u�|�W�V�����̌v�Z���Ă���
                    handCards[i] = Instantiate(cardPrefab, initPos, Quaternion.identity, parent);
                    if (isPlayer)
                    {
                        handCards[i].InitCard(cardData, SelectCard);
                        Debug.Log("�v���C���[InitCard����ID" + cardData.CardModel.cardID);
                    }
                    else
                    {
                        handCards[i].InitCard(cardData, null);
                        Debug.Log("�����InitCard����ID" + cardData.CardModel.cardID);
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
        Debug.Log("select�J�[�h�^�C�v" + SelectCardObject.Data.CardModel.cardType);     //�J�[�h�^�C�v��Attack
        Debug.Log("select�J�[�hID" + SelectCardObject.Data.CardModel.cardID);           //ID�̓N���b�N�����J�[�h��ID
        for (int i = 0; i < handCards.Length; ++i)
        {
            if (handCards[i] == selected)
            {
                handCards[i] = null;    //�J�[�h�o����
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
        Life -= damagePoint;    //MathF��Βl�ɂ����ق��������H
        ApplyUI() ;
    }
    //�J�[�h�h���[���̎�D���̔z�u�|�W�V�����v�Z
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
