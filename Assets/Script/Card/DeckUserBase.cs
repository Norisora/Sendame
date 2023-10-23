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
        Life = 3;
    }

    public virtual void TurnStart()
    {
        foreach (var v in handCards)
        {
            if (v != null)
            {
                v.ApplyCard(ChargeCount);
                Debug.Log("�`���[�W�J�E���g" + ChargeCount);
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
            var cardData = deckData.PassCard();     //�f�b�L�̈�ԏ��CardData��n��
            for (int i = 0; i < handCards.Length; i++)
            {
                if (handCards[i] == null)
                {
                    Vector2 initPos = InitPosCalc(parent, i);   //InitPosCalc�Ŕz�u�|�W�V�����̌v�Z���Ă���
                    handCards[i] = Instantiate(cardPrefab, initPos, Quaternion.identity, parent);
                    if (isPlayer)
                    {
                        handCards[i].InitCard(cardData, SelectCard);
                        Debug.Log("�v���C���[InitCard����ID" + cardData.ID);
                    }
                    else
                    {
                        handCards[i].InitCard(cardData, null);
                        Debug.Log("�����InitCard����ID" + cardData.ID);
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
        SelectCardObject.Data.Type = cardController.Data.Type;
        Debug.Log("select�J�[�h�^�C�v" + SelectCardObject.Data.Type);     //�J�[�h�^�C�v��Attack
        Debug.Log("select�J�[�hID" + SelectCardObject.Data.ID);           //ID�̓N���b�N�����J�[�h��ID
        IsTurnEnd = true;
    }
    public void Charge(int chargeCount)
    {
        ChargeCount += chargeCount;
        Debug.Log("�`���[�W�I" +  ChargeCount);
    }

    public void GetDamage(int cardAttackPoint)
    {
        Life -= cardAttackPoint;
    }
    //�J�[�h�h���[���̎�D���̔z�u�|�W�V�����v�Z
    Vector2 InitPosCalc(Transform parent, int i)
    {
        float posX = parent.position.x - firstCardPos + (i * cardSpacing);

        return new Vector2(posX, parent.position.y);
    }
}
