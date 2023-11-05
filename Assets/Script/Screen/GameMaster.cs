using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;

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

    int[] playerDeckData = { 400, 2, 3, 500, 400, 6, 3, 2, 3, 1, 2, 3, 1, 2, 3, };
    int[] enemyDeckData = { 6, 3, 3, 7, 2, 1, 1, 2, 3, 1, 2, 3, 1, 2, 3, };
    //[4]A30[5]A60[6]S30[7]S60

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
            if(player.Life == 0 && enemy.Life == 0)
            {
                state = State.Tie;
                break;
            }
            if (player.Life == 0)
            {
                state = State.PlayerLose;
                break;
            }
            if (enemy.Life == 0)
            {
                state = State.PlayerWin;
                break;
            }
            player.TurnStart();
            Debug.Log("�v���C���[�^�[��");
            enemy.TurnStart();
            Debug.Log("�G�l�~�[�^�[��");

            yield return SendText(yourTurnText);

            yield return player.Turn();
            Debug.Log("�v���C���[�^�[���̏I��" + player.SelectCardObject.Data.CardModel.cardType);
            yield return enemy.Turn();
            Debug.Log("�G�l�~�[�^�[���̏I��");
            yield return BattlePart();
        }

        //���U���g������
        if (state == State.Tie)  //�Ђ��킯
        {
            GameDirector.Instance.TransitionManager.
                TransitionScreen(ConstScreenList.ScreenType.Main);
        }
        if (state == State.PlayerLose)  //�v���C���[�܂�
        {
            yield return SendText(youLose);
            GameDirector.Instance.TransitionManager.
                TransitionScreen(ConstScreenList.ScreenType.Title);
        }
        if (state == State.PlayerWin)  //�v���C���[����
        {
            yield return SendText(youWin);
            GameDirector.Instance.TransitionManager.
                TransitionScreen(ConstScreenList.ScreenType.GameOver);
        }

        //=============
    }

    IEnumerator BattlePart()
    {
        Debug.Log("�o�g���p�[�g");
        var playerCard = player.SelectCardObject.Data.CardModel;
        var enemyCard = enemy.SelectCardObject.Data.CardModel;
        var playerType = playerCard.cardType;
        var enemyType = enemyCard.cardType;
        player.MoveToField(player.SelectCardObject, playerField);
        enemy.MoveToField(enemy.SelectCardObject, enemyField);
        yield return new WaitForSeconds(1.0f);

        if (playerType == CardType.Attack)
        {
            player.Charge(-playerCard.needChargeValue);
            if (enemyType != CardType.Shield) 
            {
                enemy.GetDamage(playerCard.attackValue - enemyCard.shieldValue);    //�G�l�~�[�̃_���[�W
                
                Debug.Log("enemy�̃_���[�W");
            }
        }
        if (enemyType == CardType.Attack)
        {
            enemy.Charge(-enemyCard.needChargeValue);
            if(playerType != CardType.Shield)
            {
                player.GetDamage(enemyCard.attackValue - playerCard.shieldValue);    //�v���C���[�̃_���[�W
                
                Debug.Log("player�̃_���[�W");
            }
        }

        if (player.Life <= 0 && enemy.Life <= 0)
        {
            state = State.Tie;
            Debug.Log("�Ђ��킯");
            yield break;
        }
        if(player.Life <= 0)
        {
            state = State.PlayerLose;
            Debug.Log("player�̂܂�");
            yield break;
        }
        if(enemy.Life <= 0)
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
    public IEnumerator SendText(TextMeshProUGUI text)
    {
        //�^�[�����\��
        float y = text.rectTransform.localScale.y;
        text.rectTransform.DOLocalMoveX(0, 0.5f);
        yield return new WaitForSeconds(2.0f);
        text.rectTransform.DOLocalMoveX(-2500, 0.5f);
        yield return new WaitForSeconds(0.5f);
    }
}
