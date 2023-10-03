using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainScreen : ScreenBase
{
    [SerializeField]
    Button next;
    [SerializeField]
    TextMeshProUGUI text;


    private void Start()
    {
        text.text = "Main";
        next.onClick.AddListener(() => GameDirector.Instance.TransitionManager.TransitionScreen(ConstScreenList.ScreenType.GameOver));
    }

}
