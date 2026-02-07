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
}
