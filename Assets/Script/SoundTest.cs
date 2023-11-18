using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{

    [SerializeField]
    private AudioClip clip1; //音源データ1

    [SerializeField]
    private AudioClip clip2; //音源データ2


    void Update()
    {
        if (Input.GetMouseButton(0)) //左クリック
        {
            Debug.Log("左クリック");
            //GameDirector.Instance.SoundManager.Play(SoundName.Save); //サウンドマネージャーを使用して効果音再生
        }
        if (Input.GetMouseButtonDown(1)) //右クリック
        {
            Debug.Log("右クリック");
            //GameDirector.Instance.SoundManager.Play("huri"); //サウンドマネージャーを使用して効果音再生
        }
    }
}
