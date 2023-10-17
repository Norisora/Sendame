using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObject : MonoBehaviour, IPointerDownHandler
{
    Action<CardObject> SelectCard;

    public bool IsActive { get; private set; }
    public CardDataTest Data { get; private set; }

    public void InitCard(CardDataTest cardData, Action<CardObject> selectCard)
    {
        SelectCard = selectCard;
        Data = cardData;
        //�`�`�`�`�`
    }

    public void ApplyCard(int currentCharge)
    {
        IsActive = currentCharge >= Data.NeedChargeValue;   //���߂邪�K�v������Ύg����
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsActive) return;

        if (SelectCard != null)
        {
            SelectCard(this);
        }
    }
}
