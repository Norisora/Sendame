using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    [SerializeField]
    DeckBuilding deckBuilding;
    [SerializeField]
    Transform parent;

    public void OnDrop(PointerEventData eventData) // �h���b�v���ꂽ���ɍs������
    {
        Debug.Log("OnDrop");
        CardDrag card = eventData.pointerDrag.GetComponent<CardDrag>(); // �h���b�O���Ă��������擾
        if (card != null)
        {
            card.transform.SetParent(parent);
            var cardID = card.gameObject.GetComponent<CardController>().Data.CardModel.cardID;
            deckBuilding.GetID(cardID);
        }
        //var raycastResults = new List<RaycastResult>();


        //EventSystem.current.RaycastAll(eventData, raycastResults);

        //foreach (var hit in raycastResults)
        //{
        // ���� DroppableField �̏�Ȃ�A���̈ʒu�ɌŒ肷��
                //card.parentRectTransform = this.transform as RectTransform; // �J�[�h�̐e�v�f�������i
        //}
        //CardDrag card = eventData.pointerDrag.GetComponent<CardDrag>(); // �h���b�O���Ă��������擾
        //if (card != null) // �����J�[�h������΁A
        //{
        //    card.parent = this.transform; // �J�[�h�̐e�v�f�������i�A�^�b�`����Ă�I�u�W�F�N�g�j�ɂ���
        //}
    }
}
