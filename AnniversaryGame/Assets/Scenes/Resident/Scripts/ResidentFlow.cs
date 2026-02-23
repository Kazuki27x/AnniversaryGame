using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResidentFlow : BaseScene
{
    [SerializeField] private TextWindow m_textWindow;
    [SerializeField] private Loading m_loading;

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
        m_loading.gameObject.SetActive(false);
    }

    public async UniTask GotoNextScene(string sceneName)
    {
        m_loading.StartFadeIn();
        await UniTask.WaitUntil(() => !m_loading.m_isFade);
        await UniTask.Delay(System.TimeSpan.FromSeconds(8));
        // フェード終了で次のシーンへ
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        // アクティブシーンとResidentシーン以外は削除
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (!(SceneManager.GetSceneAt(i).name == SceneManager.GetActiveScene().name) &&
                !(SceneManager.GetSceneAt(i).name == GameUtility.NAME_RESIDENT_SCENE))
            {
                await SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }
    }


    public async UniTask LoadingFadeOut()
    {
        m_loading.StartFadeOut();
        await UniTask.WaitUntil(() => !m_loading.m_isFade);
        // フェード終了
    }

    public TextWindow GetTextWindow()
    {
        return m_textWindow;
    }

    public Loading GetLoadingw()
    {
        return m_loading;
    }
}
