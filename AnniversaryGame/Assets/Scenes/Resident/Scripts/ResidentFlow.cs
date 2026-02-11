using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResidentFlow : BaseScene
{
    [SerializeField]
    private TextWindow m_textWindow;

    // 起動時に実行される
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RuntimeInitializaOnLoadResidentScene()
    {
        // ResidentScene作成
        SceneManager.LoadScene(GameUtility.NAME_RESIDENT_SCENE, LoadSceneMode.Additive);
    }

    private void Awake()
    {
        GameManager.Instance.SetResidentFlow(this);
    }

    protected override async UniTask OnSceneReadyAsync(CancellationToken token)
    {
        // シーン起動時の処理
    }

    public TextWindow GetTextWindow()
    {
        return m_textWindow;
    }
}
