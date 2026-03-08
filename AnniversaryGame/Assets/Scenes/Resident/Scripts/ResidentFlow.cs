using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResidentFlow : BaseScene
{
    [SerializeField] private TextWindow m_textWindow;

    [SerializeField] private Loading m_loadingOyasumi;
    [SerializeField] private Loading m_loadingWhiteOut;
    private Loading m_currentLoadingObj;

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
        m_loadingOyasumi.gameObject.SetActive(false);
        m_loadingWhiteOut.gameObject.SetActive(false);
        m_currentLoadingObj = m_loadingOyasumi; // 一旦おやすみローディングを入れておく
    }

    public async UniTask GotoNextScene(string sceneName, GameUtility.LoadFadeType loadType = GameUtility.LoadFadeType.OyasumiLoad)
    {
        GameManager.Instance.SetInputSystemAllDisable();

        float loadingTime = 6;
        switch (loadType)
        {
            case GameUtility.LoadFadeType.OyasumiLoad:
                m_currentLoadingObj = m_loadingOyasumi;
                loadingTime = 6;
                break;
            case GameUtility.LoadFadeType.WhiteLoad:
                m_currentLoadingObj = m_loadingWhiteOut;
                loadingTime = 2;
                break;
            default:
                m_currentLoadingObj = m_loadingOyasumi;
                loadingTime = 6;
                break;
        }
        m_currentLoadingObj.StartFadeIn();
        await UniTask.WaitUntil(() => !m_currentLoadingObj.m_isFade);
        await UniTask.Delay(System.TimeSpan.FromSeconds(loadingTime));
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
        if (m_currentLoadingObj.m_isLoading)
        {
            m_currentLoadingObj.StartFadeOut();
            await UniTask.WaitUntil(() => !m_currentLoadingObj.m_isFade);
        }
    }

    public TextWindow GetTextWindow()
    {
        return m_textWindow;
    }
}
