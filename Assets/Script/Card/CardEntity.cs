using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]
public class CardEntity : ScriptableObject
{
    public enum CardType
    {
        Attack,
        Shield,
        Charge,
    };
    public int cardID;
    public CardType cardType;
    public int needChargeValue;
    public string cardName;
    public Sprite icon;
}
