using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CardType
{
    Attack,
    Shield,
    Charge,
};

[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]
public class CardEntity : ScriptableObject
{
    public int cardID;
    public Sprite icon;
    public CardType cardType;
    public int needChargeValue;
    public string cardName;
    public int attackValue;
    public int shieldValue;
}
