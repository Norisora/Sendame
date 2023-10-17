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

        // リザルト-----

        //--------------
    }

    IEnumerator BattlePart()
    {
        var playerType = player.SelectCardObject.Data.Type;
        var enemyType = enemy.SelectCardObject.Data.Type;


        if (playerType == CardType.Attack && enemyType != CardType.Defence)
        {
            // Enemyのダメージ.
        }
        if (enemyType == CardType.Attack && playerType != CardType.Defence)
        {
            // Playerのダメージ.
        }

        if (player.Life <= 0)
        {
            // プレイヤーのまけ
            state = State.PlayerLose;
            yield break;
        }
        if (enemy.Life <= 0)
        {
            // エネミーのまけ
            state = State.PlayerWin;
            yield break;
        }

        if (playerType == CardType.Charge)
        {
            // Playerチャージ
            player.AddCharge(1);
        }
        if (enemyType == CardType.Charge)
        {
            // Enemyチャージ
            enemy.AddCharge(1);
        }



        // ～～～～～

        // ～～～～～

        yield break;
    }

}
