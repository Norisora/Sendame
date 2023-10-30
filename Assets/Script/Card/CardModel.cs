using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    public int cardID;
    public string name;
    public int needChargeValue;
    public CardType cardType;
    public Sprite icon;
    

    public CardModel(int cardID)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);

        this.cardID = cardEntity.cardID;
        cardType = cardEntity.cardType;
        name = cardEntity.cardName;
        needChargeValue = cardEntity.needChargeValue;
        icon = cardEntity.icon;
    }
}