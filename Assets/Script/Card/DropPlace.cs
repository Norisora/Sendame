using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler, IPointerDownHandler , IPointerUpHandler
{
    [SerializeField]
    DeckBuilding deckBuilding;
    [SerializeField]
    Transform parent;

    public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
    {
        MoveCard(eventData.pointerDrag.transform, true);
        Debug.Log("OnDrop");
    }

    public void MoveCard(Transform card, bool isDrop = false)
    {
        if (card != null)
        {
            card.transform.SetParent(parent);
            
            if(isDrop )
            {
                var cardID = card.gameObject.GetComponent<CardController>().Data.CardModel.cardID;
                deckBuilding.GetID(cardID);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("ポインターダウン");
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
