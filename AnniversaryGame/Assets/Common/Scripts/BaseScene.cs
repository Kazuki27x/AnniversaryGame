using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.SceneManagement;

public abstract class BaseScene : MonoBehaviour
{

    // Start is called before the first frame update
    private async UniTaskVoid Start()
    {
        var token = this.GetCancellationTokenOnDestroy();
        await InitializeAsync();
        await OnSceneReadyAsync(token);
    }

    // 例：BGM マネージャーのセットアップ、UI の共通初期化など
    protected virtual UniTask InitializeAsync()
    {

        return UniTask.CompletedTask;
    }

    protected abstract UniTask OnSceneReadyAsync(CancellationToken token);

    // テキストウィンドウ
    public async UniTask StartTextWindow(string csvFileName, CancellationToken token)
    {
        // InputSystemの動作を制限
        GameManager.Instance._InputControls.Player.Disable();
        GameManager.Instance._InputControls.TextWindow.Enable();
        // テキストウィンドウ表示
        List<TextContentData> tmpList = new List<TextContentData>();
        tmpList = await GameManager.Instance._CSVLoader.LoadStoryCSVAsync(csvFileName, token);
        GameManager.Instance.m_ResidentFlow.GetTextWindow().SetTextContents(tmpList);
        await GameManager.Instance.m_ResidentFlow.GetTextWindow().PlayTextWindow(token);
        // ストーリー終了待ち
        await UniTask.WaitUntil(() => !isPlayTextWindow());
        // 終了
        GameManager.Instance._InputControls.Player.Enable();
        GameManager.Instance._InputControls.TextWindow.Disable();
    }
    public bool isPlayTextWindow()
    {
        return GameManager.Instance.m_ResidentFlow.GetTextWindow().m_nowPlay;
    }
}
