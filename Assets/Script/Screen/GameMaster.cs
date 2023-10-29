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

        player.deckData.CreateData(playerDeckData);   //�f�b�L����
        enemy.deckData.CreateData(enemyDeckData);

        Debug.Log("�v���C���[�h���[�R��");
        player.DrawCard(3, playerHand);
        Debug.Log("�G�l�~�[�h���[�R��");
        enemy.DrawCard(3, enemyHand);
        while(state == State.Battle)
        {
            Debug.Log("�v���C���[�h���[1��");
            player.DrawCard(1, playerHand);
            Debug.Log("�G�l�~�[�h���[1��");
            enemy.DrawCard(1, enemyHand);
            player.TurnStart();
            Debug.Log("�v���C���[�^�[��");
            enemy.TurnStart();
            Debug.Log("�G�l�~�[�^�[��");

            yield return player.Turn();
            Debug.Log("�v���C���[�^�[���̏I��" + player.SelectCardObject.Data.CardModel.cardType);
            yield return enemy.Turn();
            Debug.Log("�G�l�~�[�^�[���̏I��");
            yield return BattlePart();
        }

        //���U���g������
        if (state == State.PlayerLose)  //�v���C���[�܂�
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
                enemy.GetDamage(1);    //�G�l�~�[�̃_���[�W
                Debug.Log("enemy�̃_���[�W");
            }
        }
        if (enemyType == CardType.Attack)
        {
            enemy.Charge(-1);
            if(playerType != CardType.Shield)
            {
                player.GetDamage(1);    //�v���C���[�̃_���[�W
                Debug.Log("player�̃_���[�W");
            }
        }

        if (player.Life <= 0 && enemy.Life <= 0)
        {
            state = State.Tie;
            Debug.Log("�Ђ��킯");
            yield break;
        }
        if(player.Life <= 0 || playerHandCards == null)
             {
                state = State.PlayerLose;
                Debug.Log("player�̂܂�");
                yield break;
             }
        if(enemy.Life <= 0 || enemyHandCards == null)
             {
                state = State.PlayerWin;
                Debug.Log("�v���C���[�̂���");
                yield break;
             }

        if(playerType == CardType.Charge)
        {
            player.Charge(1);
            Debug.Log("Player�`���[�W�I" + player.ChargeCount);
        }
        if(enemyType == CardType.Charge)
        {
            enemy.Charge(1);
            Debug.Log("Enemy�`���[�W�I" + enemy.ChargeCount);
        }

        //

        //
        yield break;
    }
}
