using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScreen : ScreenBase
{
    [SerializeField]
    Button next;
    [SerializeField]
    TextMeshProUGUI text;


    private void Start()
    {
        text.text = "Title";
        next.onClick.AddListener(() => GameDirector.Instance.TransitionManager.TransitionScreen(ConstScreenList.ScreenType.Main));
    }

}
