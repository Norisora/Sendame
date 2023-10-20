using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public enum CardType
    {
        Attack,
        Shield,
        Charge,
    };

    public int ID { get; set; }

    public CardType Type { get; set; }

    public int NeedChargeValue { get; set; }
}
