using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataTest
{
    public enum CardType
    {
        Attack,
        Defence,
        Charge,

    };

    public int ID { get; set; }

    public CardType Type { get; set; }

    public int NeedChargeValue { get; set; }
}
