using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    public int cardID;
    public string name;
    public Sprite icon;

    public CardModel(int cardID)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);

        cardID = cardEntity.cardID;
        name = cardEntity.cardName;
        icon = cardEntity.icon;
    }
}