using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeckData
{
    List<CardData> cards = new List<CardData>();
    public List<int> deck = new ();   //デッキ構成
    public void CreateData(int[] data)
    {
        int[] Data = data;

        foreach (var v in Data)
        {
            var cardData = new CardData();
            cardData.CreateCard(v);
            cards.Add(cardData);
        }
    }

    public List<int> Choice(int cardID)
    {
        deck.Add(cardID);
        return deck;
    }

    public void Building()     //ボタン押したらデッキ構築し保存
    {
        CreateData(deck.ToArray());
        //プレイヤープレフスUnityEngineつく他のクラスで実行
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
    public int GetDeckCount()
    {
        return cards.Count();
    }
}