using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.InputSystem;
using UniRx;

public class TitleFlow : BaseScene
{
    // キーアクション
    private InputAction _pushStart;
    private bool m_isEndPush = false;

    protected override async UniTask OnSceneReadyAsync(CancellationToken token)
    {
        // シーン起動時の処理
        /// 試しにステージ名を変更する
        GameManager.Instance.StageName = "StageTitle";

        await UniTask.WaitUntil(() => GameManager.Instance.m_isInitializeResidentFlow);

        // キー操作登録
        GameManager.Instance.SetInputSystemAllDisable();

        // フェードアウト待ち
        await GameManager.Instance.m_ResidentFlow.LoadingFadeOut();

        GameManager.Instance._InputControls.Title.Enable();
        SetInputAction().AddTo(token);
    }

    private System.IDisposable SetInputAction()
    {
        // スタート
        _pushStart = GameManager.Instance._InputControls.Title.PushStart;
        _pushStart.started += PushStart;

        return Disposable.Create(() =>
        {
            _pushStart.started -= PushStart;
        });
    }

    private async void PushStart(InputAction.CallbackContext ctx)
    {
        if (!m_isEndPush)
        {
            m_isEndPush = true;
            await GameManager.Instance.m_ResidentFlow.GotoNextScene("Stage1");
        }
    }
}
