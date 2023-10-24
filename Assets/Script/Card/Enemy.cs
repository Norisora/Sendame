using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : DeckUserBase
{
    Player player;
    public override void TurnStart()
    {
        base.TurnStart();
        SelectPart();
    }
    public void SelectPart()
    {
        if(player.ChargeCount > 0)
        {
            for(int i = 0; i < handCards.Length; ++i)
            {
                handCards[i] = handCards.FirstOrDefault(Type => Type.Data.CardModel.cardType == CardType.Shield);
                //ヒットしたカードタイプの手札にアプライプライオリティ
                handCards[i].ApplyPriority(1);
            }

        }
    }
    public override IEnumerator Turn()
    {
        base.Turn();
        yield return null;

        //SelectCard(handCards.OrderBy(Priority).Last());   //手札の中の
    }
}
