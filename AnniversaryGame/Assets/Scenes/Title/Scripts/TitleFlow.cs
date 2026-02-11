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

    protected override async UniTask OnSceneReadyAsync(CancellationToken token)
    {
        // シーン起動時の処理
        /// 試しにステージ名を変更する
        GameManager.Instance.StageName = "StageTitle";

        // キー操作登録
        GameManager.Instance.SetInputSystemAllDisable();
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

    private void PushStart(InputAction.CallbackContext ctx)
    {
        GotoNextScene("Stage1");
    }
}
