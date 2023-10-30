using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : DeckUserBase
{
    [SerializeField]
    Player player;
    public override void TurnStart()
    {
        base.TurnStart();
        SelectPart();
    }
    public void SelectPart()
    {
        // TODO �o�O�@�v���C���[�`���[�W�Q�G�l�~�[�`���[�W�O�̎��@Attack�I��
        var selectCards = handCards.Where(card => card != null).ToArray();    //null�łȂ���D�̔z����
        if (0 < ChargeCount)
        {
            Debug.Log("�G�l�~�[�`���[�W�J�E���g0����" + ChargeCount);
            var attackCards = selectCards.Where(type => type.Data.CardModel.cardType == CardType.Attack).ToArray();
            foreach (var card in attackCards)
            {
                card.ApplyPriority(2);
                Debug.Log($"Card Priority: {card.Priority}");
                Debug.Log("ApplyPriority�J�[�hType" + card.Data.CardModel.cardType);
            }
        }
        if (ChargeCount <= 0)
        {
            Debug.Log("�G�l�~�[�̃`���[�W�O�ȉ� Attack�I��s��");
            //Attack�ȊO�̃J�[�h��I��
            var noAttackCards = selectCards.Where(type => type.IsActive == true).ToArray();
            //noAttackCards�̒�����I��
            if (0 < player.ChargeCount)
            {
                Debug.Log("�v���C���[�`���[�W�J�E���g0����");
                var shieldCards = noAttackCards.Where(
                    type => type.Data.CardModel.cardType == CardType.Shield).ToArray();
                //�q�b�g�����J�[�h�^�C�v�̎�D�ɃA�v���C�v���C�I���e�B
                foreach (var card in shieldCards)
                {
                    card.ApplyPriority(1);
                    Debug.Log($"Card Priority: {card.Priority}");
                    Debug.Log("ApplyPriority�J�[�hType" + card.Data.CardModel.cardType);
                }
            }
            else
            {
                Debug.Log("�v���C���[�`���[�W�J�E���g0");
                var chargeCards = noAttackCards.Where(
                    type => type.Data.CardModel.cardType == CardType.Charge).ToArray();
                foreach (var card in chargeCards)
                {
                    card.ApplyPriority(1);
                    Debug.Log($"Card Priority: {card.Priority}");
                    Debug.Log("ApplyPriority�J�[�hType" + card.Data.CardModel.cardType);
                }
            }
        }

        //�v���C�I���e�B��0��

    }
    public override IEnumerator Turn()
    {
        base.Turn();
        yield return null;
        if(handCards != null && handCards.Any())
        {
            //��D�̒���Priority����ԍ������̂�I��
            var cards = handCards.Where(card => card != null && card.IsActive).ToArray();
            if (!cards.Any() )
            {
                yield break;
            }
            SelectCard(cards.OrderBy(card => card.Priority).Last());
            foreach (var card in cards)
            {
                card.PriorityZero();
            }
        }
        else
        {
            Debug.LogError("EnemyHands No Cards!");
        }
    }
}
