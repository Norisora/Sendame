using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{

    [SerializeField]
    private AudioClip clip1; //�����f�[�^1

    [SerializeField]
    private AudioClip clip2; //�����f�[�^2


    void Update()
    {
        if (Input.GetMouseButton(0)) //���N���b�N
        {
            Debug.Log("���N���b�N");
            //GameDirector.Instance.SoundManager.Play(SoundName.Save); //�T�E���h�}�l�[�W���[���g�p���Č��ʉ��Đ�
        }
        if (Input.GetMouseButtonDown(1)) //�E�N���b�N
        {
            Debug.Log("�E�N���b�N");
            //GameDirector.Instance.SoundManager.Play("huri"); //�T�E���h�}�l�[�W���[���g�p���Č��ʉ��Đ�
        }
    }
}
