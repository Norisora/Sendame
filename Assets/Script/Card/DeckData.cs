using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeckData
{
    List<CardData> cards = new List<CardData>();

    public void CreateData(int[] data)
    {
        int[] Data = data;

        foreach (var v in Data)
        {
            cards.Add(new CardData() { ID = v });
        }
    }

    public CardData PassCard()
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