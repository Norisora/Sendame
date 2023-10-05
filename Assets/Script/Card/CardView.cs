using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] SpriteRenderer iconImage;

    public void Show(CardModel cardModel)
    {
        nameText.text = cardModel.name;
        iconImage.sprite = cardModel.icon;
    }
}
