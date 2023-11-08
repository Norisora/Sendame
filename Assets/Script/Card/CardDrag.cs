using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    public RectTransform parentRectTransform;
    public Transform cardParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = rectTransform.parent as RectTransform;
    }

    private Vector2 prevPos;

    public void OnBeginDrag(PointerEventData eventData) // �h���b�O���n�߂�Ƃ��ɍs������
    {
        Debug.Log("OnBeginDrag");
        prevPos = rectTransform.anchoredPosition;   //�h���b�O�O�̈ʒu���L�����Ă���
        cardParent = transform.parent;

        transform.SetParent(cardParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycasts���I�t�ɂ���
    }

    public void OnDrag(PointerEventData eventData) // �h���b�O���Ă鎞�ɋN��������
    {
        Debug.Log("OnDrag");
        Vector2 localPos = GetLocalPosition(eventData.position);
        rectTransform.anchoredPosition = localPos;
        //transform.position = localPos;
    }

    public void OnEndDrag(PointerEventData eventData) // �J�[�h�𗣂����Ƃ��ɍs������
    {
        Debug.Log("OnEndDrag");
        rectTransform.anchoredPosition = prevPos;
        transform.SetParent(cardParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycasts���I���ɂ���
    }
    private Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);
        return result;
    }
}
