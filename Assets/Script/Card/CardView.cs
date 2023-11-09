using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] TMP_Text nameText, needChargeValue, attackValue, shieldValue;
    [SerializeField] SpriteRenderer iconImage;
    [SerializeField] Image image;

    public void Show(CardModel cardModel)
    {
        nameText.text = cardModel.name;
        
        if(cardModel.attackValue == 0)
        {
            attackValue.gameObject.SetActive(false);
        }
        else
        {
            attackValue.text = cardModel.attackValue.ToString();
        }
        if(cardModel.shieldValue == 0)
        {
            shieldValue.gameObject.SetActive(false);
        }
        else
        {
            shieldValue.text = cardModel.shieldValue.ToString();
        }
        if(cardModel.needChargeValue == 0)
        {
            needChargeValue.gameObject.SetActive(false);
        }
        else
        {
            needChargeValue.text = cardModel.needChargeValue.ToString();
        }
        if(iconImage != null)
        {
            iconImage.sprite = cardModel.icon;
        }
        if (image != null)
        {
            image.sprite = cardModel.icon;
        }
    }
}
