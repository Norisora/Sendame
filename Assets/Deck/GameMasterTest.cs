using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardDataTest;

public class GameMasterTest : MonoBehaviour
{
    public enum State
    {
        Battle,
        PlayerWin,
        PlayerLose,

    }

    [SerializeField]
    PlayerTest player;
    [SerializeField]
    EnemyTest enemy;

    State state;

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        state = State.Battle;
        // 
        player.DrawCard(3);
        enemy.DrawCard(3);
        while (state == State.Battle)
        {
            player.DrawCard(1);
            enemy.DrawCard(1);
            player.TrunStart();
            enemy.TrunStart();

            yield return player.Turn();
            yield return enemy.Turn();

            yield return BattlePart();
        }

        // ���U���g-----

        //--------------
    }

    IEnumerator BattlePart()
    {
        var playerType = player.SelectCardObject.Data.Type;
        var enemyType = enemy.SelectCardObject.Data.Type;


        if (playerType == CardType.Attack && enemyType != CardType.Defence)
        {
            // Enemy�̃_���[�W.
        }
        if (enemyType == CardType.Attack && playerType != CardType.Defence)
        {
            // Player�̃_���[�W.
        }

        if (player.Life <= 0)
        {
            // �v���C���[�̂܂�
            state = State.PlayerLose;
            yield break;
        }
        if (enemy.Life <= 0)
        {
            // �G�l�~�[�̂܂�
            state = State.PlayerWin;
            yield break;
        }

        if (playerType == CardType.Charge)
        {
            // Player�`���[�W
            player.AddCharge(1);
        }
        if (enemyType == CardType.Charge)
        {
            // Enemy�`���[�W
            enemy.AddCharge(1);
        }



        // �`�`�`�`�`

        // �`�`�`�`�`

        yield break;
    }

}
