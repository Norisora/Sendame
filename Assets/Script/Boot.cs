using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField]
    ConstScreenList.ScreenType bootScreen;

    void Start()
    {
        GameDirector.Instance.TransitionManager.TransitionScreen(bootScreen);

        Destroy(this.gameObject);
    }
}
