using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : ScreenBase
{
    [SerializeField]
    Button next;
    [SerializeField]
    TextMeshProUGUI text;


    private void Start()
    {
        text.text = "GameOverScreen";
        next.onClick.AddListener(() => GameDirector.Instance.TransitionManager.TransitionScreen(ConstScreenList.ScreenType.Title));
    }

}
