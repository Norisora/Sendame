using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameDirector : SingletonBehaviour<GameDirector>
{
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    TransitionManager transitionManager;

    public Camera MainCameara => mainCamera;
    public TransitionManager TransitionManager => transitionManager;

    protected override void OnCreateSingleton()
    {
        DontDestroyOnLoad(this);
    }

}
