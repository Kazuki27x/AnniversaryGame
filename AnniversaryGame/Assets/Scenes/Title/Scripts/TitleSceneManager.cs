using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.InputSystem;

public class TitleSceneManager : BaseScene
{
    // キーアクション
    private InputAction _pushStart;

    protected override async UniTask OnSceneReadyAsync(CancellationToken token)
    {
        CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();

        // シーン起動時の処理
        /// 試しにステージ名を変更する
        GameManager.Instance.StageName = "StageTitle";

        // キー操作登録
        SetInputAction();
    }

    void OnDestroy()
    {
        // キー登録を解放する
        _pushStart.performed -= PushStart;
    }

    private void SetInputAction()
    {
        // スタート
        _pushStart = GameManager.Instance._InputControls.Title.PushStart;
        _pushStart.started += PushStart;
    }

    private void PushStart(InputAction.CallbackContext ctx)
    {
        GotoNextScene("Stage1");
    }
}
