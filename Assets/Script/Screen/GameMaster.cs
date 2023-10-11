using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
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

    bool isPlayerTurn = true;
    List<int> deck = new List<int>() {1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };

    CardController[] playerHandCardList = new CardController[5];

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        SetStartHand(); //３枚配る

        TurnCalc();     //ターン開始１枚ドロー
    }

    void CreateCard(int cardID, Transform place, int i)
    {
        float cardSpacing = 5.0f;


        float posX = place.position.x - 11 + (emptyIndex * cardSpacing);
            
        Vector2 initPos = new Vector2(posX, place.position.y);
        CardController card = Instantiate(cardPrefab, initPos,Quaternion.identity, place); ;
        card.Init(cardID);

    }

    void SetStartHand()     //カードを３枚配る
    {
        for (int i = 0; i < 3; i++)
        {
            DrawCard(playerHand);
        }
    }
    void DrawCard(Transform hand)   //カードをドローする
    {
        if(!deck.Any())     //カードが１枚もないとき
        {
            return;
        }

        for(int i = 0; i < playerHandCardList.Length; i++)
        {
            if (playerHandCardList[i] == null)
            {
                int cardID = deck.First();
                deck.RemoveAt(0);

                //Transform emptySlot = FindEmptySlot(hand);
                //if(emptySlot != null)
                CreateCard(cardID, hand, i);
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
        DrawCard(playerHand);   //手札を一枚加える
    }
    void EnemyTurn()
    {
        Debug.Log("Enemyのターン");
        CreateCard(1, enemyField);
        ChangeTurn();   //ターンエンド
    }
}
