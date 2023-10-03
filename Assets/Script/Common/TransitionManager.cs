using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ConstScreenList;

public class TransitionManager : MonoBehaviour
{
    [SerializeField]
    Image fadeImage;

    ConstScreenList.ScreenType currentScreenType = ConstScreenList.ScreenType.None;
    ScreenBase currentScreen = null;
    Coroutine fadeCoroutine;
    public Color FadeColor { get; set; } = Color.white;

    public void TransitionScreen(ConstScreenList.ScreenType screenType)
    {
        if (currentScreen != null)
        {
            currentScreen.SetBlockRaycast(true);
        }

        if (currentScreen == null)
        {
            fadeCoroutine = StartCoroutine(FadeIn(0.0f, () => Load(screenType)));
        }
        else
        {
            fadeCoroutine = StartCoroutine(FadeIn(0.5f, () => Load(screenType)));
        }
    }

    void Load(ConstScreenList.ScreenType screenType)
    {
        var go = Resources.Load(ConstScreenList.ScreenPaths[screenType]) as GameObject;
        if (go == null)
        {
            Debug.LogError($"LoadError : screenType[{screenType}]");
            return;
        }

        if (currentScreen != null)
        {
            Destroy(currentScreen.gameObject);
            currentScreen = null;
        }
        currentScreen = Instantiate(go).GetComponent<ScreenBase>();
        currentScreen.SetBlockRaycast(true);
        fadeCoroutine = StartCoroutine(FadeOut(0.5f, () => currentScreen.SetBlockRaycast(false)));
    }



    IEnumerator FadeIn(float duration, Action endCallback)
    {
        float time = 0.0f;
        while (time < duration)
        {
            var color = FadeColor;
            color.a = time / duration;

            fadeImage.color = color;

            yield return null;
            time += Time.deltaTime;
        }
        {
            var color = FadeColor;
            color.a = 1.0f;
            fadeImage.color = color;
        }
        fadeCoroutine = null;
        endCallback?.Invoke();
    }
    IEnumerator FadeOut(float duration, Action endCallback)
    {
        float time = 0.0f;
        while (time < duration)
        {
            var color = FadeColor;
            color.a = 1.0f - time / duration;

            fadeImage.color = color;

            yield return null;
            time += Time.deltaTime;
        }
        {
            var color = FadeColor;
            color.a = 0.0f;
            fadeImage.color = color;
        }
        fadeCoroutine = null;
        endCallback?.Invoke();
    }
}
