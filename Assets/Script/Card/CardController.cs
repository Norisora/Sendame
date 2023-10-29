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

    public void InitCard(CardData cardData, Action<CardController> selectCard)    //カード生成時に呼ばれる関数
    {
        SelectCard = selectCard;
        Data = cardData;

        CardView.Show(Data.CardModel);       // TODO カードの生成山札0になるとエラー発生
    }

    //チャージ数が足りていたらAttackを使えるようにする
    public void ApplyCard(int currentCharge)
    {
        IsActive = currentCharge >= Data.CardModel.needChargeValue;
    }
    
    //プライオリティ優先度の加減
    public void ApplyPriority(int value)
    {
        Priority += value;
    }
    public void PriorityZero()
    {
        Priority = 0;
        Debug.Log("プライオリティを0に");
    }
}
