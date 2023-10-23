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
    State state;    //

    bool isPlayerTurn = true;
    List<int> PlayerDeck = new List<int>() { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };

    CardController[] playerHandCardList;
    CardController[] enemyHandCardList;

    int[] playerDeckData = { 1, 2, 3, 3, 2, 1, 1, 2, 3, 1, 2, 3, 1, 2, 3, };

    int[] enemyDeckData = { 1, 3, 3, 2, 2, 1, 1, 2, 3, 1, 2, 3, 1, 2, 3, };
    bool playerCanAttack = false;
    bool enemyCanAttack = false;


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
            Debug.Log("プレイヤーターンの終了" + player.SelectCardObject.Data.Type);
            yield return enemy.Turn();
            Debug.Log("エネミーターンの終了");
            yield return BattlePart();
        }

        //リザルト＝＝＝
        //=============
    }

    IEnumerator BattlePart()
    {
        var playerType = player.SelectCardObject.Data.Type;
        var enemyType = enemy.SelectCardObject.Data.Type;

        if (playerType == CardType.Attack && enemyType != CardType.Shield)
        {
            enemy.GetDamage(1);    //エネミーのダメージ
        }
        if (enemyType == CardType.Attack && playerType != CardType.Shield)
        {
            player.GetDamage(1);    //プレイヤーのダメージ
        }

        if(player.Life <= 0)
        {
            state = State.PlayerLose;
            yield break;
        }
        if(enemy.Life <= 0)
        {
            state = State.PlayerWin;
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

    void StartGame()
    {
        playerHandCardList = new CardController[5];
        enemyHandCardList = new CardController[5];
        SetStartHand(playerHandCardList, playerHand); //３枚配る
        SetStartHand(enemyHandCardList, enemyHand);
    }

    CardController CreateCard(int cardID, Transform place, int i)
    {
        float cardSpacing = 5.0f;
        float posX = place.position.x - 11.0f + (i * cardSpacing);

        Vector2 initPos = new Vector2(posX, place.position.y);
        CardController card = Instantiate(cardPrefab, initPos, Quaternion.identity, place); ;
        //card.InitCard(cardID);
        return card;
    }

    void SetStartHand(CardController[] handCardList, Transform hand)     //カードを３枚配る
    {
      

        for (int ii = 0; ii < 3; ii++)
        {
            DrawCard(handCardList,hand);
        }

    }
    void DrawCard(CardController[] handCardList, Transform hand)   //カードをドローする
    {
        if (!PlayerDeck.Any())     //カードが１枚もないとき
        {
            return;
        }

        for (int i = 0; i < handCardList.Length; i++)
        {
            if (handCardList[i] == null)
            {
                int cardID = PlayerDeck.First();
                PlayerDeck.RemoveAt(0);

                //Transform emptySlot = FindEmptySlot(hand);
                //if(emptySlot != null)
                handCardList[i] = CreateCard(cardID, hand, i);
                break;
            }
        }
    }

  
    
    //Transform FindEmptySlot(Transform hand)
    //{
    //    // 手札内のすべてのスロットを取得
    //    Transform[] cardSlots = hand.GetComponentsInChildren<Transform>();

    //    // 最初のスロットから順に空いているかチェック
    //    foreach (Transform slot in cardSlots)
    //    {
    //        // カードが配置されていない（同じ親でない）スロットを空いているスロットとみなす
    //        if (slot != hand && slot.childCount == 0)
    //        {
    //            return slot; // 空いているスロットを返す
    //        }
    //    }

    //    return null; // 空きが見つからない場合は null を返す
    //}
    //void ArrangeHandSlots(Transform hand)
    //{
    //    // 手札のスロット数
    //    int numSlots = 5;

    //    // スロット間の間隔
    //    float slotSpacing = 2.0f; // 任意の間隔に設定してください

    //    // 最初のスロットのX座標
    //    float startX = -((numSlots - 1) * slotSpacing) / 2.0f;

    //    // スロットを均等に配置
    //    for (int i = 0; i < numSlots; i++)
    //    {
    //        // スロットの位置を計算して設定
    //        float slotX = startX + i * slotSpacing;
    //        Vector3 slotPosition = new Vector3(slotX, hand.position.y, hand.position.z);

    //        // スロットの位置を設定
    //        // ここで手札のスロットにカードを配置する処理も追加できます
    //        // ...

    //        // カードを配置する場合、カードの位置を設定して親を手札に設定
    //         CardController card = Instantiate(cardPrefab, slotPosition, Quaternion.identity);
    //         card.transform.SetParent(hand);
    //    }
    //}


    void TurnCalc()     //ターンを管理する
    {
        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            EnemyTurn();
        }
    }

    public void ChangeTurn() //ターンエンドボタンにつける処理
    {
        isPlayerTurn = !isPlayerTurn;   //ターンを逆にする
        TurnCalc();     //ターンを相手に回す
    }

    void PlayerTurn()
    {
        Debug.Log("Playerのターン");
        DrawCard(playerHandCardList,playerHand);   //手札を一枚加える
    }
    void EnemyTurn()
    {
        Debug.Log("Enemyのターン");
        DrawCard(enemyHandCardList,enemyHand);
        ChangeTurn();   //ターンエンド
    }
}
