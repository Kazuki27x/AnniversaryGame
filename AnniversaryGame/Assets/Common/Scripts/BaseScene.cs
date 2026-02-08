using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.SceneManagement;

public abstract class BaseScene : MonoBehaviour
{
    protected ResidentScene m_ResidentScene;

    // Start is called before the first frame update
    private async UniTaskVoid Start()
    {
        var token = this.GetCancellationTokenOnDestroy();
        bool isExistResident = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name.Equals(GameUtility.NAME_RESIDENT_SCENE))
            {
                isExistResident = true;
            }
        }
        if (!isExistResident)
        {
            SceneManager.LoadScene(GameUtility.NAME_RESIDENT_SCENE, LoadSceneMode.Additive);
        }

        var scene = SceneManager.GetSceneByName(GameUtility.NAME_RESIDENT_SCENE);
        await UniTask.WaitUntil(() => scene.isLoaded);
        if (scene.isLoaded)
        {
            foreach (var root in scene.GetRootGameObjects()) 
            { 
                if (root.name == "ResidentFlow")
                {
                    m_ResidentScene = root.GetComponent<ResidentScene>();
                }
            }
        }

        await InitializeAsync();
        await OnSceneReadyAsync(token);
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

    // テキストウィンドウ
    public async UniTask StartTextWindow(string csvFileName, CancellationToken token)
    {
        List<TextContentData> tmpList = new List<TextContentData>();
        tmpList = await GameManager.Instance._CSVLoader.LoadStoryCSVAsync(csvFileName, token);
        m_ResidentScene.GetTextWindow().SetTextContents(tmpList);
        await m_ResidentScene.GetTextWindow().PlayTextWindow(token);
    }
    public bool isPlayTextWindow()
    {
        return m_ResidentScene.GetTextWindow().m_nowPlay;
    }
}
