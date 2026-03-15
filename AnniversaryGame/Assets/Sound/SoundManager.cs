using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_bgmAudioSource;
    [SerializeField] private AudioSource m_seAudioSource;

    [Header("BGM")]
    [SerializeField] private AudioClip BGM_TITLE;
    [SerializeField] private AudioClip BGM_STAGE;

    [Header("SE")]
    [SerializeField] private AudioClip SE_TITLE_PUSH;
    [SerializeField] private AudioClip SE_SELECT;
    [SerializeField] private AudioClip SE_TEXT;
    [SerializeField] private AudioClip SE_MOVE_STAGE;

    private static readonly float DEFAULT_BGM_VOLUME = 0.3f;

    public enum BGM_TYPE
    {
        TITLE,
        STAGE,
    }
    public enum SE_TYPE
    {
        TITLE_PUSH,
        SELECT,
        TEXT,
        MOVE_STAGE,
    }

    public void PlayBGM(BGM_TYPE type)
    {
        StopBGM().Forget();

        AudioClip clip = null;
        switch (type)
        {
            case BGM_TYPE.TITLE:
                clip = BGM_TITLE;
                break;
            case BGM_TYPE.STAGE:
                clip = BGM_STAGE;
                break;
        }
        if (clip != null)
        {
            Debug.Log($"PlayBGM {clip.name}");
            m_bgmAudioSource.clip = clip;
            m_bgmAudioSource.volume = DEFAULT_BGM_VOLUME;
            m_bgmAudioSource.Play();
        }
    }

    public async UniTask StopBGM(bool isFade = false)
    {
        if (isFade)
        {
            var tcs = new UniTaskCompletionSource();
            DG.Tweening.DOVirtual.Float(DEFAULT_BGM_VOLUME, 0, 1, value =>
            {
                m_bgmAudioSource.volume = value;
            }).OnComplete(() => tcs.TrySetResult());
            await tcs.Task;
        }
        m_bgmAudioSource.Stop();
    }

    public void PlaySE(SE_TYPE type)
    {
        m_seAudioSource.Stop();

        AudioClip clip = null;
        switch (type)
        {
            case SE_TYPE.TITLE_PUSH:
                clip = SE_TITLE_PUSH;
                break;
            case SE_TYPE.SELECT:
                clip = SE_SELECT;
                break;
            case SE_TYPE.TEXT:
                clip = SE_TEXT;
                break;
            case SE_TYPE.MOVE_STAGE:
                clip = SE_MOVE_STAGE;
                break;
        }
        if (clip != null)
        {
            Debug.Log($"PlaySE {clip.name}");
            m_seAudioSource.PlayOneShot(clip);
        }
    }
}
