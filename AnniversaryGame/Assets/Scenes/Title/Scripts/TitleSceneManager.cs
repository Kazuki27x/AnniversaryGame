using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleSceneManager : BaseScene
{
    protected override async UniTask OnSceneReadyAsync()
    {
        // シーン起動時の処理
        /// 試しにステージ名を変更する
        GameManager.Instance.StageName = "StageTitle";
    }
}
