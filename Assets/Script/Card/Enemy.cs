using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : DeckUserBase
{
    [SerializeField]
    Player player;
    public override void TurnStart()
    {
        base.TurnStart();
        SelectPart();
    }
    public void SelectPart()
    {
        if (player.ChargeCount > 0)
        {
            var cards = handCards.Where(
                Type => Type.Data.CardModel.cardType == CardType.Shield);
            //ヒットしたカードタイプの手札にアプライプライオリティ
            foreach (var card in cards)
            {
                card.ApplyPriority(1);
                Debug.Log($"Card Priority: {card.Priority}");
                Debug.Log("ApplyPriorityカードType" + card.Data.CardModel.cardType);
            }
        }
        else return;
    }
    public override IEnumerator Turn()
    {
        base.Turn();
        yield return null;
        if(handCards != null && handCards.Any())
        {
            //手札の中のPriorityが一番高いものを選択
            var cards = handCards.OrderBy(card => card != null).ToArray();
            SelectCard(cards.OrderBy(card => card.Priority).Last());
        }
        else
        {
            SelectCard(handCards[0]);
        }
        
    }
}
