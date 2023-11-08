using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeckData
{
    List<CardData> cards = new List<CardData>();
    int[] deck = new int[30];   //30���̃f�b�L�\��
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

    public int[] Choice(int cardID)
    {
        deck.ToList().Add(cardID);
        return deck;
    }

    void Building()     //�{�^����������f�b�L�\�z���ۑ�
    {
        CreateData(deck);
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