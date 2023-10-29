using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CardData;

public class GameMaster : MonoBehaviour
{
    public enum State       //
    {
        Battle,
        PlayerWin,
        PlayerLose,
        Tie,
    }

    [SerializeField]
    CardController cardPrefab;
    [SerializeField]
    Transform enemyHand;
    [SerializeField]
    Transform enemyField;
    [SerializeField]
    Transform playerField;
    [SerializeField]
    Transform playerHand;

    [SerializeField]
    Player player;
    [SerializeField]
    Enemy enemy;
    State state;

    int[] playerDeckData = { 1, 2, 3, 3, 2, 1, 1, 2, 3, 1, 2, 3, 1, 2, 3, };
    int[] enemyDeckData = { 1, 3, 3, 2, /*2, 1, 1, 2, 3, 1, 2, 3, 1, 2, 3, */};

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        state = State.Battle;

        player.deckData.CreateData(playerDeckData);   //デッキ生成
        enemy.deckData.CreateData(enemyDeckData);

        Debug.Log("プレイヤードロー３枚");
        player.DrawCard(3, playerHand);
        Debug.Log("エネミードロー３枚");
        enemy.DrawCard(3, enemyHand);
        while(state == State.Battle)
        {
            Debug.Log("プレイヤードロー1枚");
            player.DrawCard(1, playerHand);
            Debug.Log("エネミードロー1枚");
            enemy.DrawCard(1, enemyHand);
            player.TurnStart();
            Debug.Log("プレイヤーターン");
            enemy.TurnStart();
            Debug.Log("エネミーターン");

            yield return player.Turn();
            Debug.Log("プレイヤーターンの終了" + player.SelectCardObject.Data.CardModel.cardType);
            yield return enemy.Turn();
            Debug.Log("エネミーターンの終了");
            yield return BattlePart();
        }

        //リザルト＝＝＝
        if (state == State.PlayerLose)  //プレイヤーまけ
        {
            GameDirector.Instance.TransitionManager.
                TransitionScreen(ConstScreenList.ScreenType.GameOver);
        }
        //=============
    }

    IEnumerator BattlePart()
    {
        var playerHandCards = player.GetHandCards();
        var enemyHandCards = enemy.GetHandCards();
        
        var playerType = player.SelectCardObject.Data.CardModel.cardType;
        player.MoveToField(player.SelectCardObject, playerField);
        var enemyType = enemy.SelectCardObject.Data.CardModel.cardType;
        enemy.MoveToField(enemy.SelectCardObject, enemyField);
        yield return new WaitForSeconds(1);

        if (playerType == CardType.Attack)
        {
            player.Charge(-1);
            if (enemyType != CardType.Shield) 
            { 
                enemy.GetDamage(1);    //エネミーのダメージ
                Debug.Log("enemyのダメージ");
            }
        }
        if (enemyType == CardType.Attack)
        {
            enemy.Charge(-1);
            if(playerType != CardType.Shield)
            {
                player.GetDamage(1);    //プレイヤーのダメージ
                Debug.Log("playerのダメージ");
            }
        }

        if (player.Life <= 0 && enemy.Life <= 0)
        {
            state = State.Tie;
            Debug.Log("ひきわけ");
            yield break;
        }
        if(player.Life <= 0 || playerHandCards == null)
             {
                state = State.PlayerLose;
                Debug.Log("playerのまけ");
                yield break;
             }
        if(enemy.Life <= 0 || enemyHandCards == null)
             {
                state = State.PlayerWin;
                Debug.Log("プレイヤーのかち");
                yield break;
             }

        if(playerType == CardType.Charge)
        {
            player.Charge(1);
            Debug.Log("Playerチャージ！" + player.ChargeCount);
        }
        if(enemyType == CardType.Charge)
        {
            enemy.Charge(1);
            Debug.Log("Enemyチャージ！" + enemy.ChargeCount);
        }

        //

        //
        yield break;
    }
}
