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
    [SerializeField]
    SoundManager soundManager;

    public Camera MainCamera => mainCamera;
    public TransitionManager TransitionManager => transitionManager;

    protected override void OnCreateSingleton()
    {
        DontDestroyOnLoad(this);
    }

}
