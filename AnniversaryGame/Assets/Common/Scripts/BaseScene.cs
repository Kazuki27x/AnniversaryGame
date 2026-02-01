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
        InitializeAsync();
        OnSceneReadyAsync(token);
    }

    // 例：BGM マネージャーのセットアップ、UI の共通初期化など
    protected virtual UniTask InitializeAsync()
    {

        return UniTask.CompletedTask;
    }

    protected abstract UniTask OnSceneReadyAsync(CancellationToken token);

    protected void GotoNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
