using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    public int cardID;
    public Sprite icon;
    public CardType cardType;
    public string name;
    public int needChargeValue;
    public int attackValue;
    public int shieldValue;

    public CardModel(int cardID)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);

        this.cardID = cardEntity.cardID;
        icon = cardEntity.icon;
        cardType = cardEntity.cardType;
        name = cardEntity.cardName;
        needChargeValue = cardEntity.needChargeValue;
        attackValue = cardEntity.attackValue;
        shieldValue = cardEntity.shieldValue;
    }
}