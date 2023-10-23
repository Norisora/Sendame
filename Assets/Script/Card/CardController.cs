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
    public CardModel CardModel { get; private set; }

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

        CardModel = new CardModel(Data.ID);  //カードIDを元にScriptableObjectからカードデータを取得
        Data.Type = CardModel.cardType;     //ここに追加
        CardView.Show(CardModel);       //カードの生成
        Debug.Log("InitCard CardDataのData.Type" + Data.Type);
    }

    //チャージ数が足りていたらAttackを使えるようにする
    public void ApplyCard(int currentCharge)
    {
        IsActive = currentCharge >= Data.NeedChargeValue;
    }
    
    //プライオリティ優先度の加減
    public void ApplyPriority()
    {

    }
}
