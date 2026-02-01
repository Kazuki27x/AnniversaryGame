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

    // キーアクション
    private InputAction _pushStart;

    protected override async UniTask OnSceneReadyAsync(CancellationToken token)
    {
        // シーン起動時の処理
        GameManager.Instance.StageName = "Stage1";
    }
}
