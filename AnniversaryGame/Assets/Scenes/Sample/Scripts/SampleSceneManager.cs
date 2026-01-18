using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class SampleSceneManager : BaseScene
{

    [SerializeField] SpriteRenderer m_Player;

    protected override async UniTask OnSceneReadyAsync(CancellationToken token)
    {
        // シーン起動時の処理
        /// ステージ名を変更する
        GameManager.Instance.StageName = "StageSample";
        /// Player画像変更

        Sprite ImageAsset = await AddressableAssetLoadUtility.LoadAssetAsync<Sprite>("shibaFront.png", token);
        m_Player.sprite = ImageAsset;
    }
}
