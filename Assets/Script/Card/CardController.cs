using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour
{
    Action<CardController> SelectCard;
    public bool IsActive { get; private set; }
    public CardData Data { get; private set; }
    public CardView CardView { get; private set; }

    public int Priority { get; private set; }
   
    private void Awake()
    {
        CardView = GetComponent<CardView>();
    }
    

    public void OnMouseDown()
    {
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

        CardView.Show(Data.CardModel);       // TODO �J�[�h�̐����R�D0�ɂȂ�ƃG���[����
    }

    //�`���[�W��������Ă�����Attack���g����悤�ɂ���
    public void ApplyCard(int currentCharge)
    {
        IsActive = currentCharge >= Data.CardModel.needChargeValue;
    }
    
    //�v���C�I���e�B�D��x�̉���
    public void ApplyPriority(int value)
    {
        Priority += value;
    }
    public void PriorityZero()
    {
        Priority = 0;
        Debug.Log("�v���C�I���e�B��0��");
    }
}
