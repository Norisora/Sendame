using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    RectTransform parentRectTransform;
    Transform cardParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = rectTransform.parent as RectTransform;
    }

    private Vector2 prevPos;

    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {
        Debug.Log("OnBeginDrag");
        prevPos = rectTransform.localPosition;   //ドラッグ前の位置を記憶しておく
        //cardParent = transform.parent;
        GameObject card = this.gameObject;
        Instantiate(card, transform.parent);
        //transform.SetParent(cardParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycastsをオフにする
    }

    public void OnDrag(PointerEventData eventData) // ドラッグしてる時に起こす処理
    {
        Vector2 localPos = GetLocalPosition(eventData.position);
        rectTransform.localPosition = localPos;
        //transform.position = localPos;
    }

    public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
    {
        Debug.Log("OnEndDrag");
        rectTransform.localPosition = prevPos;
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
    }
    private Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);
        return result;
    }
}
