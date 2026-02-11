using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Building : MonoBehaviour
{
    [Header("対象CSVファイル名")]
    public string storyFileName = "";

    // 表示が終了したか（ストーリー表示は一度きり）
    public bool m_isFinishDisp { get; set; } = false;

    public void IsFinishDisp()
    {
        m_isFinishDisp = true;
    }
}
