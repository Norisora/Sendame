using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    //BGM�Ǘ�
    public enum BGMType
    {
        // BGM�p�̗񋓎q���Q�[���ɍ��킹�ēo�^
        DeckBuilding,
        Battle,
        Win,
        Lose,
        Tie,
        SILENCE = 999,        // ������Ԃ�BGM�Ƃ��č쐬�������ꍇ�ɂ͒ǉ����Ă����B����ȊO�͕s�v
    }
    // SE�Ǘ�
    public enum SEType
    {
        // SE�p�̗񋓎q���Q�[���ɍ��킹�ēo�^
        Save,
        Attack,
        Shield,
        Charge,
    }
    public enum OneShotType
    {
        // SE�p�̗񋓎q���Q�[���ɍ��킹�ēo�^
        //oneshot�͂ǂ�����ăv���C
        Win,
        Lose,
        Tie,
    }

    // �N���X�t�F�[�h����
    public const float CROSS_FADE_TIME = 1.0f;

    // �{�����[���֘A
    public float BGMVolume = 0.1f;
    public float SEVolume = 0.2f;
    public bool Mute = false;

    // === AudioClip ===
    public AudioClip[] BGMClips;
    public AudioClip[] SEClips;
    public AudioClip[] OneShotClips;

    // SE�pAudioMixer  ���g�p
    //public AudioMixer audioMixer;


    // === AudioSource ===
    private AudioSource[] BGMSources = new AudioSource[2];
    private AudioSource[] SESources = new AudioSource[16];
    private AudioSource[] OneShotSources = new AudioSource[2];

    private bool isCrossFading;

    private int currentBgmIndex = 999;

    private void Awake()
    {
        //// �V���O���g�����A�V�[���J�ڂ��Ă��j������Ȃ��悤�ɂ���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //BGM�pAudioSource�ǉ�
        BGMSources[0] = gameObject.AddComponent<AudioSource>();
        BGMSources[1] = gameObject.AddComponent<AudioSource>();

        //SE�pAudioSource�ǉ�
        for(int i = 0; i < SESources.Length;  i++)
        {
            SESources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        //�{�����[���ݒ�
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
    /// BGM�Đ�
    /// </summary>
    /// <param name="bgmType"></param>
    /// <param name="isLoop"></param>
    public void PlayBGM(BGMType bGMType, bool isLoop = true)
    {
        //BGM�Ȃ��̏�Ԃɂ���Ƃ�
        if((int)bGMType == 999)
        {
            StopBGM();
            return;
        }

        int index = (int)bGMType;
        currentBgmIndex = index;

        if(index < 0 || BGMClips.Length <= index) return;
        
        //����BGM�̏ꍇ�͉������Ȃ�
        if (BGMSources[0].clip != null 
            && BGMSources[0].clip == BGMClips[index]) return;
        else if(BGMSources[1].clip !=  null 
            && BGMSources[1].clip == BGMClips[index]) return;
       

        //�t�F�[�h��BGM�J�n
        if (BGMSources[0].clip == null && BGMSources[1].clip == null)
        {
            BGMSources[0].loop = isLoop;
            BGMSources[0].clip = BGMClips[index];
            BGMSources[0].Play();
        }
        else
        {
            //�N���X�t�F�[�h����
            StartCoroutine(CrossFadeChangeBGM(index, isLoop));
        }
    }

    /// <summary>
    /// BGM�̃N���X�t�F�[�h����
    /// </summary>
    /// <param name="index">AudioClip�̔ԍ�</param>
    /// <param name="isLoop">���[�v�ݒ�B���[�v���Ȃ��ꍇ����false�w��</param>
    /// <returns></returns>
    IEnumerator CrossFadeChangeBGM(int index, bool isLoop)
    {
        isCrossFading = true;
        if (BGMSources[0].clip != null)
        {
            //[0]���Đ�����Ă���Ƃ��A[0]�̉��ʂ����X�ɉ�����[1]��V�����ȂƂ��čĐ�
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
            //[1]���Đ�����Ă���Ƃ��A[1]�̉��ʂ����X�ɉ�����[0]��V�����ȂƂ��čĐ�
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
    /// BGM���S��~
    /// </summary>
    public void StopBGM()
    {
        BGMSources[0].Stop();
        BGMSources[1].Stop();
        BGMSources[0].clip = null;
        BGMSources[1].clip = null;
    }

    /// <summary>
    /// SE�Đ�
    /// </summary>
    /// <param name="seType"></param>
    public int PlaySE(SEType seType)
    {
        int index = (int)seType;
        if(index < 0 || SEClips.Length <= index) return -1;
        
        for(int i = 0; i < SESources.Length; ++i)
        {
            var source = SESources[i];
            //�Đ�����AudioSource�̂Ƃ��͎��̃��[�v������
            if (source.isPlaying) continue;

            //�Đ����łȂ�AudioSource��Clip���Z�b�g����SE��炷
            source.clip = SEClips[index];
            source.Play();
            return i;
        }
        return -1;  //�G���[
    }

    /// <summary>
    /// SE��~
    /// </summary>
    public void StopSE()
    {
        //�S�Ă�SE�p��AudioSource���~����
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


        //�Đ����łȂ�AudioSource���g����SE��炷
        foreach (AudioSource source in OneShotSources)
        {
            //�Đ�����AudioSource�̂Ƃ��͎��̃��[�v������
            if (source.isPlaying) continue;

            //�Đ����łȂ�AudioSource��Clip���Z�b�g����SE��炷
            source.PlayOneShot(OneShotClips[index]);    //���ʉ��炷
            break;
        }
    }

    /// <summary>
    /// BGM�ꎞ��~
    /// </summary>
    public void MuteBGM()
    {
        BGMSources[0].Stop();
        BGMSources[1].Stop();
    }

    /// <summary>
    /// �ꎞ��~��������BGM���Đ�(�ĊJ)
    /// </summary>
    public void ResumeBGM()
    {
        BGMSources[0].Play();
        BGMSources[1].Play();
    }
}