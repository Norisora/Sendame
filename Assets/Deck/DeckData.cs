using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckData
{
    List<CardData> cards = new List<CardData>();

    public void CreateTestData()
    {
        int[] testDatas = { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };

        foreach (var v in testDatas)
        {
            cards.Add(new CardData() { ID = v });
        }
        
    }

    public CardData DrawCard()
    {
        if (!cards.Any())
        {
            return null;
        }
        var ret = cards.First();
        cards.RemoveAt(0);
        return ret;
    }
}
