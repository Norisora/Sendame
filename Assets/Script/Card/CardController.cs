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

    private void Awake()
    {
        CardView = GetComponent<CardView>();
    }

    public void InitCard(CardData cardData, Action<CardController> selectCard)    //カード生成時に呼ばれる関数
    {
        SelectCard = selectCard;
        Data = cardData;
        int cardID = Data.ID;
        CardModel = new CardModel(cardID);  //カードデータを取得
        CardView.Show(CardModel);       //カードの生成
    }

    public void ApplyCard(int currentBuildUp)
    {
        IsActive = currentBuildUp >= Data.NeedBuildUpValue;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!IsActive) return;

        if(SelectCard != null)
        {
            SelectCard(this);
        }
    }
}
