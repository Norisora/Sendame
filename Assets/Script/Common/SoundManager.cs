using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    //BGM管理
    public enum BGMType
    {
        // BGM用の列挙子をゲームに合わせて登録
        DeckBuilding,
        Battle,
        Win,
        Lose,
        Tie,
        SILENCE = 999,        // 無音状態をBGMとして作成したい場合には追加しておく。それ以外は不要
    }
    // SE管理
    public enum SEType
    {
        // SE用の列挙子をゲームに合わせて登録
        Save,
        Attack,
        Shield,
        Charge,
    }
    public enum OneShotType
    {
        // SE用の列挙子をゲームに合わせて登録
        //oneshotはどうやってプレイ
        Win,
        Lose,
        Tie,
    }

    // クロスフェード時間
    public const float CROSS_FADE_TIME = 1.0f;

    // ボリューム関連
    public float BGMVolume = 0.1f;
    public float SEVolume = 0.2f;
    public bool Mute = false;

    // === AudioClip ===
    public AudioClip[] BGMClips;
    public AudioClip[] SEClips;
    public AudioClip[] OneShotClips;

    // SE用AudioMixer  未使用
    //public AudioMixer audioMixer;


    // === AudioSource ===
    private AudioSource[] BGMSources = new AudioSource[2];
    private AudioSource[] SESources = new AudioSource[16];
    private AudioSource[] OneShotSources = new AudioSource[2];

    private bool isCrossFading;

    private int currentBgmIndex = 999;

    private void Awake()
    {
        //// シングルトンかつ、シーン遷移しても破棄されないようにする
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //BGM用AudioSource追加
        BGMSources[0] = gameObject.AddComponent<AudioSource>();
        BGMSources[1] = gameObject.AddComponent<AudioSource>();

        //SE用AudioSource追加
        for(int i = 0; i < SESources.Length;  i++)
        {
            SESources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        //ボリューム設定
        if(!isCrossFading)
        {
            BGMSources[0].volume = BGMVolume;
            BGMSources[1].volume = BGMVolume;
        }
        foreach(AudioSource source in SESources)
        {
            source.volume = SEVolume;
        }
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="bgmType"></param>
    /// <param name="isLoop"></param>
    public void PlayBGM(BGMType bGMType, bool isLoop = true)
    {
        //BGMなしの状態にするとき
        if((int)bGMType == 999)
        {
            StopBGM();
            return;
        }

        int index = (int)bGMType;
        currentBgmIndex = index;

        if(index < 0 || BGMClips.Length <= index) return;
        
        //同じBGMの場合は何もしない
        if (BGMSources[0].clip != null 
            && BGMSources[0].clip == BGMClips[index]) return;
        else if(BGMSources[1].clip !=  null 
            && BGMSources[1].clip == BGMClips[index]) return;
       

        //フェードでBGM開始
        if (BGMSources[0].clip == null && BGMSources[1].clip == null)
        {
            BGMSources[0].loop = isLoop;
            BGMSources[0].clip = BGMClips[index];
            BGMSources[0].Play();
        }
        else
        {
            //クロスフェード処理
            StartCoroutine(CrossFadeChangeBGM(index, isLoop));
        }
    }

    /// <summary>
    /// BGMのクロスフェード処理
    /// </summary>
    /// <param name="index">AudioClipの番号</param>
    /// <param name="isLoop">ループ設定。ループしない場合だけfalse指定</param>
    /// <returns></returns>
    IEnumerator CrossFadeChangeBGM(int index, bool isLoop)
    {
        isCrossFading = true;
        if (BGMSources[0].clip != null)
        {
            //[0]が再生されているとき、[0]の音量を徐々に下げて[1]を新しい曲として再生
            BGMSources[1].volume = 0;
            BGMSources[1].clip = BGMClips[index];
            BGMSources[1].loop = isLoop;
            BGMSources[1].Play();
            BGMSources[1].DOFade(1.0f, CROSS_FADE_TIME).SetEase(Ease.Linear);
            BGMSources[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            BGMSources[0].Stop();
            BGMSources[0].clip = null;
        }
        else 
        {
            //[1]が再生されているとき、[1]の音量を徐々に下げて[0]を新しい曲として再生
            BGMSources[0].volume = 0;
            BGMSources[0].clip = BGMClips[index];
            BGMSources[0].loop = isLoop;
            BGMSources[0].Play();
            BGMSources[0].DOFade(1.0f, CROSS_FADE_TIME).SetEase(Ease.Linear);
            BGMSources[1].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            BGMSources[1].Stop();
            BGMSources[1].clip = null;
        }
        isCrossFading = false;
    }

    /// <summary>
    /// BGM完全停止
    /// </summary>
    public void StopBGM()
    {
        BGMSources[0].Stop();
        BGMSources[1].Stop();
        BGMSources[0].clip = null;
        BGMSources[1].clip = null;
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="seType"></param>
    public int PlaySE(SEType seType)
    {
        int index = (int)seType;
        if(index < 0 || SEClips.Length <= index) return -1;
        
        for(int i = 0; i < SESources.Length; ++i)
        {
            var source = SESources[i];
            //再生中のAudioSourceのときは次のループ処理へ
            if (source.isPlaying) continue;

            //再生中でないAudioSourceにClipをセットしてSEを鳴らす
            source.clip = SEClips[index];
            source.Play();
            return i;
        }
        return -1;  //エラー
    }

    /// <summary>
    /// SE停止
    /// </summary>
    public void StopSE()
    {
        //全てのSE用のAudioSourceを停止する
        foreach(AudioSource source in SESources)
        {
            source.Stop();
            source.clip = null;
        }
    }
    public void StopSE(int sourceIndex)
    {
        SESources[sourceIndex].Stop();
        SESources[sourceIndex].clip = null;
    }
    public void PlayOneShot(OneShotType oneShotType)
    {
        int index = (int)oneShotType;
        if (index < 0 || OneShotClips.Length <= index) return;


        //再生中でないAudioSourceを使ってSEを鳴らす
        foreach (AudioSource source in OneShotSources)
        {
            //再生中のAudioSourceのときは次のループ処理へ
            if (source.isPlaying) continue;

            //再生中でないAudioSourceにClipをセットしてSEを鳴らす
            source.PlayOneShot(OneShotClips[index]);    //効果音鳴らす
            break;
        }
    }

    /// <summary>
    /// BGM一時停止
    /// </summary>
    public void MuteBGM()
    {
        BGMSources[0].Stop();
        BGMSources[1].Stop();
    }

    /// <summary>
    /// 一時停止した同じBGMを再生(再開)
    /// </summary>
    public void ResumeBGM()
    {
        BGMSources[0].Play();
        BGMSources[1].Play();
    }
}