using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.InputSystem;
using UniRx;

public class Stage1Flow : BaseScene
{
    [SerializeField] GameObject _player;

    [Header("初回ストーリーCSVファイル名")]
    public string storyFileName = "";

    // キーアクション
    private InputAction _pushStart;

    protected override async UniTask OnSceneReadyAsync(CancellationToken token)
    {
        // シーン起動時の処理
        GameManager.Instance.StageName = "Stage1";

        // 初回ストーリー開始
        List<TextContentData> tmpList = new List<TextContentData>();
        tmpList = await GameManager.Instance._CSVLoader.LoadStoryCSVAsync("Stage01Story00.csv", token);
        await m_ResidentScene.StartTextWindow(tmpList, token);
    }
}
