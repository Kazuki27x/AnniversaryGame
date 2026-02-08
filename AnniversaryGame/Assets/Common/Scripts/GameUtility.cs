using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 定数などの定義に利用 
 */
public static class GameUtility
{
    public const string PLAYER_NAME = "コウペンちゃん";
    public const string NARRATION_NAME = "神様コウペンちゃん";

    public const string NAME_RESIDENT_SCENE = "ResidentScene";

    public enum Stage
    {
        Stage0 = 0,
        Stage1,
        Stage2,
        Stage3,
    }
    public enum KeyItemKind
    {
        Photo = 0,
        Movie,
    }

    // ストーリー関連
    public enum SpeakPlayer
    {
        Player = 0,
        Narattion = 1,
    };
    public enum Emotion
    {
        None = -1,
        Normal = 0,
        Smile = 1,
        Surprise = 2,
    };
}


public struct TextContentData
{
    public GameUtility.SpeakPlayer m_speakPlayer { get; private set; }
    public GameUtility.Emotion m_emotion { get; private set; }
    public string m_mainText { get; private set; }
    public string m_photoAddress { get; private set; }

    public TextContentData(GameUtility.SpeakPlayer speakPlayer, GameUtility.Emotion emotion, string mainText, string photoAddress)
    {
        m_speakPlayer = speakPlayer;
        m_emotion = emotion;
        m_mainText = mainText;
        m_photoAddress = photoAddress;
    }
}