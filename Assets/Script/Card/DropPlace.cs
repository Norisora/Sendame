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

    public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
    {
        Debug.Log("OnDrop");
        CardDrag card = eventData.pointerDrag.GetComponent<CardDrag>(); // ドラッグしてきた情報を取得
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
        // もし DroppableField の上なら、その位置に固定する
                //card.parentRectTransform = this.transform as RectTransform; // カードの親要素を自分（
        //}
        //CardDrag card = eventData.pointerDrag.GetComponent<CardDrag>(); // ドラッグしてきた情報を取得
        //if (card != null) // もしカードがあれば、
        //{
        //    card.parent = this.transform; // カードの親要素を自分（アタッチされてるオブジェクト）にする
        //}
    }
}
