using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class BaseScene : MonoBehaviour
{
    // Start is called before the first frame update
    private async UniTaskVoid Start()
    {
        InitializeAsync();
        OnSceneReadyAsync();
    }

    // 例：BGM マネージャーのセットアップ、UI の共通初期化など
    protected virtual UniTask InitializeAsync()
    {

        return UniTask.CompletedTask;
    }

    protected abstract UniTask OnSceneReadyAsync();
}
