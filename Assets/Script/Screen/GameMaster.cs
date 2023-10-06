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

        int cardID = deck.First();
        deck.RemoveAt(0);
        CreateCard(cardID, hand);
    }

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
