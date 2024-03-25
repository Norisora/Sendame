using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

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
    Transform enemyHand, enemyField, playerField, playerHand;
    [SerializeField]
    TextMeshProUGUI yourTurnText, youWin, youLose;
    [SerializeField]
    Player player;
    [SerializeField]
    Enemy enemy;
    State state;

    int[] playerDeckData = { 1, 2, 3, 1, /*2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, */};
    int[] enemyDeckData = { 1, 2, 3, 1, 2, 1, 1, 2, 3, 1, 2, 3, 1, 2, 3, };
    //[4]A30[5]A60[6]S30[7]S60

    int turnCount = 1;
    DeckUserBase deckUser;
    // Start is called before the first frame update
    void Start()
    {
        yourTurnText.text = "TurnCount " + turnCount.ToString();
        if (SaveDataManager.Instance.HasSaveData(SaveDataManager.SaveType.Deck))
        {
            //LoadDeck(player);     //↓2行と同じ処理（処理共通化できる）
            player.deckData = SaveDataManager.Instance.Load<DeckData>(SaveDataManager.SaveType.Deck);
            playerDeckData = player.deckData.deck.ToArray();
            var random = new System.Random();
            playerDeckData = playerDeckData.OrderBy(x => random.Next()).ToArray();
        }

            
        SoundManager.instance.PlayBGM(SoundManager.BGMType.Battle);
        
        StartCoroutine(GameLoop());
    }

    void LoadDeck(DeckUserBase deckUser)
    {
        deckUser.deckData = SaveDataManager.Instance.Load<DeckData>(SaveDataManager.SaveType.Deck);
        playerDeckData = deckUser.deckData.deck.ToArray();
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
            if(player.Life == 0 && enemy.Life == 0)
            {
                state = State.Tie;
                break;
            }
            if (player.Life == 0)
            {
                state = State.PlayerLose;
                Debug.Log("プレイヤーの負け");
                break;
            }
            if (player.deckData == null)
            {
                state = State.PlayerLose;
                Debug.Log("プレイヤーの負け");
                break;
            }
            if (enemy.Life == 0 || enemy.deckData == null)
            {
                state = State.PlayerWin;
                break;
            }
            player.TurnStart();
            Debug.Log("プレイヤーターン");
            enemy.TurnStart();
            Debug.Log("エネミーターン");

            yield return SendText(yourTurnText);

            yield return player.Turn();
            Debug.Log("プレイヤーターンの終了" + player.SelectCardObject.Data.CardModel.cardType);
            yield return enemy.Turn();
            Debug.Log("エネミーターンの終了");
            yield return BattlePart();
            turnCount++;
            yourTurnText.text = "TurnCount " + turnCount.ToString();
        }

        //リザルト＝＝＝
        if (state == State.Tie)  //ひきわけ
        {
            SoundManager.instance.PlayBGM(SoundManager.BGMType.Tie);
            GameDirector.Instance.TransitionManager.
                TransitionScreen(ConstScreenList.ScreenType.Main);
        }
        if (state == State.PlayerLose)  //プレイヤーまけ
        {
            SoundManager.instance.PlayBGM(SoundManager.BGMType.Lose);
            yield return SendText(youLose);
            GameDirector.Instance.TransitionManager.
                TransitionScreen(ConstScreenList.ScreenType.Title);
        }
        if (state == State.PlayerWin)  //プレイヤーかち
        {
            SoundManager.instance.PlayBGM(SoundManager.BGMType.Win);
            yield return SendText(youWin);
            GameDirector.Instance.TransitionManager.
                TransitionScreen(ConstScreenList.ScreenType.GameOver);
        }

        //=============
    }

    IEnumerator BattlePart()
    {
        Debug.Log("バトルパート");
        var playerCard = player.SelectCardObject.Data.CardModel;
        var enemyCard = enemy.SelectCardObject.Data.CardModel;
        var playerType = playerCard.cardType;
        var enemyType = enemyCard.cardType;
        StartCoroutine( player.MoveToField(player.SelectCardObject, playerField, null));
        StartCoroutine(enemy.MoveToField(enemy.SelectCardObject, enemyField, null));
        yield return new WaitForSeconds(1.0f);

        if (playerType == CardType.Attack)
        {
            SoundManager.instance.PlayOneShot(SoundManager.OneShotType.Attack);
            player.Charge(-playerCard.needChargeValue);
            if (enemyType != CardType.Shield) 
            {
                enemy.GetDamage(playerCard.attackValue - enemyCard.shieldValue);    //エネミーのダメージ
                
                Debug.Log("enemyのダメージ");
            }
        }
        if (enemyType == CardType.Attack)
        {
            SoundManager.instance.PlayOneShot(SoundManager.OneShotType.Attack);
            enemy.Charge(-enemyCard.needChargeValue);
            if(playerType != CardType.Shield)
            {
                player.GetDamage(enemyCard.attackValue - playerCard.shieldValue);    //プレイヤーのダメージ
                
                Debug.Log("playerのダメージ");
            }
        }

        if (player.Life <= 0 && enemy.Life <= 0)
        {
            state = State.Tie;
            Debug.Log("ひきわけ");
            yield break;
        }
        if(player.Life <= 0)
        {
            state = State.PlayerLose;
            Debug.Log("playerのまけ");
            yield break;
        }
        if(enemy.Life <= 0)
        {
            state = State.PlayerWin;
            Debug.Log("プレイヤーのかち");
            yield break;
        }

        if(playerType == CardType.Charge)
        {
            SoundManager.instance.PlayOneShot(SoundManager.OneShotType.Charge);
            player.Charge(1);
            Debug.Log("Playerチャージ！" + player.ChargeCount);
        }
        if(enemyType == CardType.Charge)
        {
            SoundManager.instance.PlayOneShot(SoundManager.OneShotType.Charge);
            enemy.Charge(1);
            Debug.Log("Enemyチャージ！" + enemy.ChargeCount);
        }

        //

        //
        yield break;
        
    }
    public IEnumerator SendText(TextMeshProUGUI text)
    {
        //ターン数表示
        float y = text.rectTransform.localScale.y;
        text.rectTransform.DOLocalMoveX(0, 0.5f);
        yield return new WaitForSeconds(2.0f);
        yield return text.rectTransform.DOLocalMoveX(-2500, 0.5f);

    }
}
