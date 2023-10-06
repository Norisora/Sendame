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


    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        SetStartHand();

        TurnCalc();
    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
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

        int cardID = deck.First();
        deck.RemoveAt(0);
        CreateCard(cardID, hand);
    }

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
