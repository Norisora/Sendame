using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerDownHandler
{
    Action<CardController> SelectCard;
    public bool IsActive { get; private set; }
    public CardData Data { get; private set; }
    public CardView CardView { get; private set; }
    public CardModel CardModel { get; private set; }

    public int Priority { get; private set; }
    private void Awake()
    {
        CardView = GetComponent<CardView>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("�I���|�C���^�[�_�E��");
        if (!IsActive) return;

        if (SelectCard != null)
        {
            SelectCard(this);
        }
    }

    public void InitCard(CardData cardData, Action<CardController> selectCard)    //�J�[�h�������ɌĂ΂��֐�
    {
        SelectCard = selectCard;
        Data = cardData;
        int cardID = Data.ID;
        CardModel = new CardModel(cardID);  //�J�[�hID������ScriptableObject����J�[�h�f�[�^���擾
        CardView.Show(CardModel);       //�J�[�h�̐���
    }

    //�`���[�W��������Ă�����Attack���g����悤�ɂ���
    public void ApplyCard(int currentCharge)
    {
        IsActive = currentCharge >= Data.NeedChargeValue;
    }
    
    //�v���C�I���e�B�D��x�̉���
    public void ApplyPriority()
    {

    }
}
