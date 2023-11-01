using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NumberAnimationGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    NumberAnimation generatePrefab;

    public void GenerateNumber(int number)
    {
        StartCoroutine(Generate(number, null));
    }
    public void GenerateNumber(int number, System.Action endCallback)
    {
        StartCoroutine(Generate(number, endCallback));
    }
    public IEnumerator Generate(int number, System.Action endCallback = null)
    {
        var numberText = number.ToString();

        var numberAnimations = new NumberAnimation[numberText.Length];  //アニメーションする文字数

        for (int i = 0; i < numberText.Length; ++i) //文字の生成
        {
            var animation = Instantiate(generatePrefab, parent.transform);
            animation.SetNumber(numberText[i].ToString());
            numberAnimations[i] = animation;
        }
        yield return null;

        var count = 0;
        for (int i = 0; i < numberAnimations.Length; ++i)
        {
            numberAnimations[i].StartAnimation( () => count++); //文字数分アニメさせる
            yield return new WaitForSeconds(0.1f);
        }

        while (count != numberAnimations.Length)
        {
            yield return null;
        }
        endCallback?.Invoke();
    }
}
