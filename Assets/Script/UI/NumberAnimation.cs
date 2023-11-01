using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NumberAnimation : MonoBehaviour
{
    const float MoveYValue = 30;

    [SerializeField]
    TextMeshProUGUI textNumber;
    [SerializeField]
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup.alpha = 0.0f;
    }
    public void SetNumber(int number)
    {
        textNumber.text = number.ToString();
    }
    public void SetNumber(string number)
    {
        textNumber.text = number;
    }

    public void StartAnimation(System.Action endCallback)
    {
        var temp = textNumber.rectTransform.localPosition;
        temp.y += MoveYValue;

        var sequence = DOTween.Sequence();

        canvasGroup.alpha = 1.0f;
        sequence.Append(textNumber.rectTransform.DOLocalMoveY(-MoveYValue, 0.15f));
        sequence.AppendInterval(0.5f);  //•¶ŽšÁ‚¦‚é‚Ü‚Å‚ÌŠÔŠu
        sequence.Append(canvasGroup.DOFade(0.0f, 0.5f));
        sequence.AppendCallback(() => {
            endCallback?.Invoke();
            Destroy(gameObject);
        });
        sequence.Play();
    }
}
