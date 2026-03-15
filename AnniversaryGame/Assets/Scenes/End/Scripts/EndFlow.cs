using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.InputSystem;
using UniRx;

public class EndFlow : BaseScene
{
    // キーアクション
    private InputAction _pushWhiteEnd;
    private InputAction _pushEnd;

    private bool m_isDisp = false;

    protected override async UniTask OnSceneReadyAsync(CancellationToken token)
    {
        // シーン起動時の処理
        /// 試しにステージ名を変更する
        GameManager.Instance.StageName = "EndFlow";

        await UniTask.WaitUntil(() => GameManager.Instance.m_isInitializeResidentFlow);

        // キー操作登録
        GameManager.Instance.SetInputSystemAllDisable();
        GameManager.Instance._InputControls.End.Enable();
        SetInputAction().AddTo(token);


    }

    private System.IDisposable SetInputAction()
    {
        // タイトルに戻る
        _pushWhiteEnd = GameManager.Instance._InputControls.End.PushWhiteEnd;
        _pushEnd = GameManager.Instance._InputControls.End.PushEnd;
        _pushWhiteEnd.started += PushWhiteEnd;
        _pushEnd.started += PushEnd;

        return Disposable.Create(() =>
        {
            _pushWhiteEnd.started -= PushWhiteEnd;
            _pushEnd.started -= PushEnd;
        });
    }

    private async void PushWhiteEnd(InputAction.CallbackContext ctx)
    {
        // フェードアウト待ち
        if (!m_isDisp)
        {
            await GameManager.Instance.m_ResidentFlow.LoadingFadeOut();
            m_isDisp = true;
        }
    }

    private async void PushEnd(InputAction.CallbackContext ctx)
    {
        if (m_isDisp)
        {
            await GameManager.Instance.m_ResidentFlow.GotoNextScene("TitleScene");
        }
    }
}
