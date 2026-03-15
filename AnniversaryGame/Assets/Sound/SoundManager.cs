using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StopBGM();

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
            m_bgmAudioSource.Play();
        }
    }

    public void StopBGM()
    {
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
