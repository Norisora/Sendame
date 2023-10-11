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
        SetStartHand(); //�R���z��

        TurnCalc();     //�^�[���J�n�P���h���[
    }

    void CreateCard(int cardID, Transform place, int i)
    {
        float cardSpacing = 5.0f;


        float posX = place.position.x - 11 + (emptyIndex * cardSpacing);
            
        Vector2 initPos = new Vector2(posX, place.position.y);
        CardController card = Instantiate(cardPrefab, initPos,Quaternion.identity, place); ;
        card.Init(cardID);

    }

    void SetStartHand()     //�J�[�h���R���z��
    {
        for (int i = 0; i < 3; i++)
        {
            DrawCard(playerHand);
        }
    }
    void DrawCard(Transform hand)   //�J�[�h���h���[����
    {
        if(!deck.Any())     //�J�[�h���P�����Ȃ��Ƃ�
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
        DrawCard(playerHand);   //��D���ꖇ������
    }
    void EnemyTurn()
    {
        Debug.Log("Enemy�̃^�[��");
        CreateCard(1, enemyField);
        ChangeTurn();   //�^�[���G���h
    }
}
