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
        // TODO　Serialize化より外のコード内の方がいい？
        //コード内だとConstScreenListのようにパス指定になる？
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
        return null;    //未使用のAudioSourceは見つからなかった
    }

    public void PlayBGM(AudioClip clip, bool isLoop)
    {
        var audioSource = GetUsedAudioSource(); //再生中でないAudioSourceを入れる
        if (audioSource == null ) return;   //再生できなかった
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        audioSource.Play();
    }
    public void PlayBGM(string name, bool isLoop)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))//管理用Dictionaryから別名で検索
        {
            PlayBGM(soundData.audioClip, isLoop);
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}→SoundManagerのinspector確認");
        }
    }
    public void PlaySE(AudioClip clip, bool isLoop)
    {
        var audioSource = GetUsedAudioSource();
        if (audioSource == null) return;   //再生できなかった
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        audioSource.Play();
    }
    public void PlaySE(string name, bool isLoop)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))//管理用Dictionaryから別名で検索
        {
            PlaySE(soundData.audioClip, isLoop);
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}→SoundManagerのinspector確認");
        }
    }
}
