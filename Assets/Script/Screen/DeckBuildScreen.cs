using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckBuildScreen : ScreenBase
{
    [SerializeField]
    Button next;
    [SerializeField]
    TextMeshProUGUI text;

    private void Start()
    {
        text.text = "DeckBuild";
        next.onClick.AddListener(() => GameDirector.Instance.TransitionManager.TransitionScreen(ConstScreenList.ScreenType.Main));
        SoundManager.instance.PlayBGM(SoundManager.BGMType.DeckBuilding);
    }
}