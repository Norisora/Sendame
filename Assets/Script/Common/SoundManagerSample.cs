using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManagerSample : MonoBehaviour
{
    public static SoundManagerSample instance;
    [System.Serializable]
    public class SoundData
    {
        // TODO�@Serialize�����O�̃R�[�h���̕��������H
        //�R�[�h������ConstScreenList�̂悤�Ƀp�X�w��ɂȂ�H
        public string name;
        public AudioClip audioClip;
    }
    [SerializeField]
    SoundData[] soundDatas;

    Dictionary<string, SoundData> soundDictionary = new ();

    [SerializeField]
    AudioSource bGMSource;
    [SerializeField]
    AudioSource[] sESources, oneShotSources;

    private void Awake()
    {
       foreach (var soundData in soundDatas)
       {
            soundDictionary.Add(soundData.name, soundData);
       }
    }
    AudioSource GetUsedAudioSource()
    {
        for(int i = 0; i< sESources.Length; i++)
        {
            if (sESources[i].isPlaying == false) return sESources[i];
        }
        return null;    //���g�p��AudioSource�͌�����Ȃ�����
    }

    public void PlayBGM(AudioClip clip, bool isLoop)
    {
        var audioSource = GetUsedAudioSource(); //�Đ����łȂ�AudioSource������
        if (audioSource == null ) return;   //�Đ��ł��Ȃ�����
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        audioSource.Play();
    }
    public void PlayBGM(string name, bool isLoop)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))//�Ǘ��pDictionary����ʖ��Ō���
        {
            PlayBGM(soundData.audioClip, isLoop);
        }
        else
        {
            Debug.LogWarning($"���̕ʖ��͓o�^����Ă��܂���:{name}��SoundManager��inspector�m�F");
        }
    }
    public void PlaySE(AudioClip clip, bool isLoop)
    {
        var audioSource = GetUsedAudioSource();
        if (audioSource == null) return;   //�Đ��ł��Ȃ�����
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        audioSource.Play();
    }
    public void PlaySE(string name, bool isLoop)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))//�Ǘ��pDictionary����ʖ��Ō���
        {
            PlaySE(soundData.audioClip, isLoop);
        }
        else
        {
            Debug.LogWarning($"���̕ʖ��͓o�^����Ă��܂���:{name}��SoundManager��inspector�m�F");
        }
    }
}
