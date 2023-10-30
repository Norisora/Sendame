using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        // TODO バグ　プレイヤーチャージ２エネミーチャージ０の時　Attack選択
        var selectCards = handCards.Where(card => card != null).ToArray();    //nullでない手札の配列作る
        if (0 < ChargeCount)
        {
            Debug.Log("エネミーチャージカウント0より大" + ChargeCount);
            var attackCards = selectCards.Where(type => type.Data.CardModel.cardType == CardType.Attack).ToArray();
            foreach (var card in attackCards)
            {
                card.ApplyPriority(2);
                Debug.Log($"Card Priority: {card.Priority}");
                Debug.Log("ApplyPriorityカードType" + card.Data.CardModel.cardType);
            }
        }
        if (ChargeCount <= 0)
        {
            Debug.Log("エネミーのチャージ０以下 Attack選択不可");
            //Attack以外のカードを選択
            var noAttackCards = selectCards.Where(type => type.IsActive == true).ToArray();
            //noAttackCardsの中から選択
            if (0 < player.ChargeCount)
            {
                Debug.Log("プレイヤーチャージカウント0より大");
                var shieldCards = noAttackCards.Where(
                    type => type.Data.CardModel.cardType == CardType.Shield).ToArray();
                //ヒットしたカードタイプの手札にアプライプライオリティ
                foreach (var card in shieldCards)
                {
                    card.ApplyPriority(1);
                    Debug.Log($"Card Priority: {card.Priority}");
                    Debug.Log("ApplyPriorityカードType" + card.Data.CardModel.cardType);
                }
            }
            else
            {
                Debug.Log("プレイヤーチャージカウント0");
                var chargeCards = noAttackCards.Where(
                    type => type.Data.CardModel.cardType == CardType.Charge).ToArray();
                foreach (var card in chargeCards)
                {
                    card.ApplyPriority(1);
                    Debug.Log($"Card Priority: {card.Priority}");
                    Debug.Log("ApplyPriorityカードType" + card.Data.CardModel.cardType);
                }
            }
        }

        //プライオリティを0に

    }
    public override IEnumerator Turn()
    {
        base.Turn();
        yield return null;
        if(handCards != null && handCards.Any())
        {
            //手札の中のPriorityが一番高いものを選択
            var cards = handCards.Where(card => card != null && card.IsActive).ToArray();
            if (!cards.Any() )
            {
                yield break;
            }
            SelectCard(cards.OrderBy(card => card.Priority).Last());
            foreach (var card in cards)
            {
                card.PriorityZero();
            }
        }
        else
        {
            Debug.LogError("EnemyHands No Cards!");
        }
    }
}
