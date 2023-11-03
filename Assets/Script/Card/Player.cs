using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : DeckUserBase
{
    [SerializeField]
    TextMeshProUGUI YourTurnText;
    public IEnumerator SendText()
    {
        float y = YourTurnText.rectTransform.localScale.y;
        YourTurnText.rectTransform.localPosition = new Vector2(0, y);
        yield return new WaitForSeconds(2.0f);
        YourTurnText.rectTransform.localPosition = new Vector2(-2500, y);
    }
}
