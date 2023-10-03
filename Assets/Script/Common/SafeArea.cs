using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]

public class SafeArea : MonoBehaviour
{
    [SerializeField]
    RectTransform panel;

    Rect lastSafeArea = default;

    private void Update()
    {
        var safeArea = Screen.safeArea;

#if UNITY_EDITOR
        if (Screen.width == 1125 && Screen.height == 2436)
        {
            safeArea.y = 102;
            safeArea.height = 2202;
        }
        if (Screen.width == 2436 && Screen.height == 1125)
        {
            safeArea.x = 132;
            safeArea.y = 63;
            safeArea.height = 1062;
            safeArea.width = 2172;
        }
#endif

        if (safeArea != lastSafeArea)
        {
            ApplySafeArea(safeArea);
        }
    }

    void ApplySafeArea(Rect safeArea)
    {
        if (panel == null) { return; }
        panel.anchoredPosition = Vector2.zero;
        panel.sizeDelta = Vector2.zero;


        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMax.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.y /= Screen.height;

        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;

    }
}
