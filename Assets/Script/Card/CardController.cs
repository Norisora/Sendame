using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView cardView;
    public CardModel cardModel;

    private void Awake()
    {
        cardView = GetComponent<CardView>();
    }

    public void Init(int CardID)    //カード生成時に呼ばれる関数
    {
        cardModel = new CardModel(CardID);  //カードデータを取得
        cardView.Show(cardModel);
    }
}
