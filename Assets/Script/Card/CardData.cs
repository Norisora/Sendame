using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public enum CardType
    {
        Attack,
        Shield,
        BuildUp,
    };

    public int ID { get; set; }

    public CardType Type { get; set; }

    public int NeedBuildUpValue { get; set; }
}
