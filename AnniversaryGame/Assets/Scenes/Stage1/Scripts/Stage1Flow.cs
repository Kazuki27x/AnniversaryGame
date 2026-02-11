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
    [SerializeField, Header("初回ストーリーを表示するか")]
    private bool m_isDispFirstStory;

    // キーアクション
    private InputAction _pushStart;

    protected override async UniTask OnSceneReadyAsync(CancellationToken token)
    {
        // シーン起動時の処理
        GameManager.Instance.StageName = "Stage1";

        // 初回ストーリー開始
        if (m_isDispFirstStory)
        {
            await StartTextWindow("Stage01Story00.csv", token);
        }

        // キー操作
        GameManager.Instance.SetInputSystemAllDisable();
        GameManager.Instance._InputControls.Player.Enable();
    }
}
