using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeckData
{
    List<CardData> cards = new List<CardData>();
    public List<int> deck = new ();   //�f�b�L�\��
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

    public void Building()     //�{�^����������f�b�L�\�z���ۑ�
    {
        CreateData(deck.ToArray());
        //�v���C���[�v���t�XUnityEngine�����̃N���X�Ŏ��s
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