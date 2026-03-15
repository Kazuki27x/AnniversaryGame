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

        await UniTask.WaitUntil(() => GameManager.Instance.m_isInitializeResidentFlow);

        // BGM
        GameManager.Instance.m_ResidentFlow.m_soundManager.PlayBGM(SoundManager.BGM_TYPE.STAGE);


        // フェードアウト待ち
        await GameManager.Instance.m_ResidentFlow.LoadingFadeOut();

        // 初回ストーリー開始
        if (m_isDispFirstStory)
        {
            await StartTextWindow(storyFileName, token);
        }

        // キー操作登録
        GameManager.Instance.SetInputSystemAllDisable();
        GameManager.Instance._InputControls.Player.Enable();

        // Loading終了
        await GameManager.Instance.m_ResidentFlow.LoadingFadeOut();
    }
}
