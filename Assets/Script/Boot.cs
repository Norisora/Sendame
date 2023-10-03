using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField]
    ConstScreenList.ScreenType bootScreen;

    void Start()
    {
        GameDirector.Instance.TransitionManager.TransitionScreen(ConstScreenList.ScreenType.Title);

        Destroy(this.gameObject);
    }
}
