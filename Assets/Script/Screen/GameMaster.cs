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

        player.deckData.CreateData();   //�f�b�L����
        enemy.deckData.CreateData();

        Debug.Log("�v���C���[�h���[�R��");
        player.DrawCard(3, playerHand);     //ID=1��5��@handCards[i].InitCard(cardData, SelectCard);���Ă���
        Debug.Log("�G�l�~�[�h���[�R��");
        enemy.DrawCard(3, enemyHand);      //���l
        while(state == State.Battle)
        {
            Debug.Log("�v���C���[�h���[1��");
            player.DrawCard(1, playerHand);     //handCards[i].InitCard(cardData, SelectCard);�̏������s���Ă��Ȃ�
            Debug.Log("�G�l�~�[�h���[1��");
            enemy.DrawCard(1, enemyHand);      //���l
            player.TurnStart();
            enemy.TurnStart();

            yield return player.Turn();
            yield return enemy.Turn();

            yield return BattlePart();
        }

        //���U���g������
        //=============
    }

    IEnumerator BattlePart()
    {
        var playerType = player.SelectCardObject.Data.Type;
        var enemyType = enemy.SelectCardObject.Data.Type;

        if (playerType == CardType.Attack && enemyType != CardType.BuildUp)
        {
            //Enemy�̃_���[�W
        }
        if (enemyType == CardType.Attack && playerType != CardType.BuildUp)
        {
            //Player�̃_���[�W
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

        if(playerType == CardType.BuildUp)
        {
            player.BuildUp(1);
        }
        if(enemyType == CardType.BuildUp)
        {
            enemy.BuildUp(1);
        }

        //

        //
        yield break;
    }

    void StartGame()
    {
        playerHandCardList = new CardController[5];
        enemyHandCardList = new CardController[5];
        SetStartHand(playerHandCardList, playerHand); //�R���z��
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

    void SetStartHand(CardController[] handCardList, Transform hand)     //�J�[�h���R���z��
    {
      

        for (int ii = 0; ii < 3; ii++)
        {
            DrawCard(handCardList,hand);
        }

    }
    void DrawCard(CardController[] handCardList, Transform hand)   //�J�[�h���h���[����
    {
        if (!PlayerDeck.Any())     //�J�[�h���P�����Ȃ��Ƃ�
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
    //    // ��D���̂��ׂẴX���b�g���擾
    //    Transform[] cardSlots = hand.GetComponentsInChildren<Transform>();

    //    // �ŏ��̃X���b�g���珇�ɋ󂢂Ă��邩�`�F�b�N
    //    foreach (Transform slot in cardSlots)
    //    {
    //        // �J�[�h���z�u����Ă��Ȃ��i�����e�łȂ��j�X���b�g���󂢂Ă���X���b�g�Ƃ݂Ȃ�
    //        if (slot != hand && slot.childCount == 0)
    //        {
    //            return slot; // �󂢂Ă���X���b�g��Ԃ�
    //        }
    //    }

    //    return null; // �󂫂�������Ȃ��ꍇ�� null ��Ԃ�
    //}
    //void ArrangeHandSlots(Transform hand)
    //{
    //    // ��D�̃X���b�g��
    //    int numSlots = 5;

    //    // �X���b�g�Ԃ̊Ԋu
    //    float slotSpacing = 2.0f; // �C�ӂ̊Ԋu�ɐݒ肵�Ă�������

    //    // �ŏ��̃X���b�g��X���W
    //    float startX = -((numSlots - 1) * slotSpacing) / 2.0f;

    //    // �X���b�g���ϓ��ɔz�u
    //    for (int i = 0; i < numSlots; i++)
    //    {
    //        // �X���b�g�̈ʒu���v�Z���Đݒ�
    //        float slotX = startX + i * slotSpacing;
    //        Vector3 slotPosition = new Vector3(slotX, hand.position.y, hand.position.z);

    //        // �X���b�g�̈ʒu��ݒ�
    //        // �����Ŏ�D�̃X���b�g�ɃJ�[�h��z�u���鏈�����ǉ��ł��܂�
    //        // ...

    //        // �J�[�h��z�u����ꍇ�A�J�[�h�̈ʒu��ݒ肵�Đe����D�ɐݒ�
    //         CardController card = Instantiate(cardPrefab, slotPosition, Quaternion.identity);
    //         card.transform.SetParent(hand);
    //    }
    //}


    void TurnCalc()     //�^�[�����Ǘ�����
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

    public void ChangeTurn() //�^�[���G���h�{�^���ɂ��鏈��
    {
        isPlayerTurn = !isPlayerTurn;   //�^�[�����t�ɂ���
        TurnCalc();     //�^�[���𑊎�ɉ�
    }

    void PlayerTurn()
    {
        Debug.Log("Player�̃^�[��");
        DrawCard(playerHandCardList,playerHand);   //��D���ꖇ������
    }
    void EnemyTurn()
    {
        Debug.Log("Enemy�̃^�[��");
        DrawCard(enemyHandCardList,enemyHand);
        ChangeTurn();   //�^�[���G���h
    }
}
