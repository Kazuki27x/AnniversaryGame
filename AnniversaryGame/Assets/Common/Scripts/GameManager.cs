using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // マネージャー変数
    public InputControls _InputControls { get; private set; }
    [NonSerialized] public CSVLoader _CSVLoader;
    [NonSerialized] public KeyItemManager _KeyItemManager;

    // シングルトン定義
    private static GameManager instance;
    public static GameManager Instance // 読み取り専用
    {
        get
        {
            // 参照時にinstanceの中身がない場合に生成する
            if (instance == null)
            {

                GameObject obj = new GameObject("GameManager");
                obj.AddComponent<GameManager>();
                instance = obj.GetComponent<GameManager>();
            }
            return instance;
        }
    }

    // 共通パラメーター
    public string PlayerName = "てすと";
    public string StageName = "Stage0";

    private async UniTaskVoid Awake()
    { 
        // Hierarchyのオブジェクトに割り当てられてしまった場合の保険
        if (instance == null) {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
            return;
        }
        await InitializeAsync();
    }
    private async UniTask InitializeAsync()
    {
        // 初期化
        _InputControls = new InputControls();
        _InputControls.Enable();
        _CSVLoader = new CSVLoader();
        _KeyItemManager = new KeyItemManager();

        var token = this.GetCancellationTokenOnDestroy();
        await _KeyItemManager.InitAsync(token);
    }
}
