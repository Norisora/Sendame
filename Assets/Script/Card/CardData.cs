using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public CardModel CardModel { get; set; }

    public void CreateCard(int ID)
    {
        CardModel = new CardModel(ID);
    }
}
