using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBase : MonoBehaviour
{
    [SerializeField]
    Canvas[] canvasList;

    CanvasGroup[] canvasGroupList;

    private void Awake()
    {
        canvasGroupList = new CanvasGroup[canvasList.Length];
        for (int i = 0; i < canvasGroupList.Length; ++i)
        {
            canvasGroupList[i] = canvasList[i].GetComponent<CanvasGroup>();
        }

        foreach (var canvas in canvasList)
        {
            canvas.worldCamera = GameDirector.Instance.MainCamera;
        }
    }

    public void SetBlockRaycast(bool isBlock)
    {
        foreach (var v in canvasGroupList)
        {
            v.blocksRaycasts = !isBlock;
        }
    }

}
