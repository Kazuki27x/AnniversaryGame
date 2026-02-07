using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ResidentScene : BaseScene
{
    [SerializeField]
    private TextWindow m_textWindow;

    protected override async UniTask OnSceneReadyAsync(CancellationToken token)
    {
        // ƒV[ƒ“‹N“®‚Ìˆ—
    }

    public async UniTask StartTextWindow(List<TextContentData> list, CancellationToken token)
    {
        m_textWindow.SetTextContents(list);
        await m_textWindow.PlayTextWindow(token);
    }
}
