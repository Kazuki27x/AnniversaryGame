using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Threading;

public class ActorController : MonoBehaviour
{
    [SerializeField] private BaseScene m_scene;
    [SerializeField] private float m_speed = 3f;
    [SerializeField] private GameObject m_Manpu;

    private SpriteRenderer spriteRenderer;

    private bool m_isGoal = false;

    // キーアクション
    /// 移動処理
    private InputAction _pushMove;
    private InputAction _pushNormal;
    private InputAction _pushFast;
    private Vector2 moveInput;
    /// 建物説明処理
    private InputAction _pushBuildEnter;

    // 衝突情報
    private bool m_isInBuilding = false; // 建物内にいるか
    private Building m_inBuildingInfo;
    private string m_nextSceneStr = "";
    private bool m_isMove = false;
    private bool m_isNormalMove = false;
    private bool m_isFastMove = false;

    CancellationToken m_token;

    private void Start()
    {
        m_token = this.GetCancellationTokenOnDestroy();

        spriteRenderer = GetComponent<SpriteRenderer>();
        // キー操作登録
        SetInputAction().AddTo(this);
        // 状態変化による実行
        /// 建物の入っているかを検知して、アイコンを表示
        this.ObserveEveryValueChanged(t => t.m_isInBuilding)
            .Subscribe(value => {
                DispMark(value); 
            }).AddTo(this);
    }

    float speed;
    private void Update()
    {
        if (m_isMove)
        {
            Vector3 move = new Vector3(moveInput.x, 0, 0);
            GetComponent<Rigidbody2D>().MovePosition(this.transform.position + move * m_speed * Time.fixedDeltaTime);
        }
    }

    private System.IDisposable SetInputAction()
    {
        // 移動処理
        {
            _pushMove = GameManager.Instance._InputControls.Player.Move; // 移動量検知用
            _pushNormal = GameManager.Instance._InputControls.Player.NormalTap; // 画面タッチ中か（タッチ中は移動）
            _pushFast = GameManager.Instance._InputControls.Player.Fast; // 早歩きか
            _pushMove.started += ActionMovePerformed;
            _pushMove.canceled += ActionStopVector2Canceled;
            _pushNormal.started += ActionNormalImputPerformed;
            _pushNormal.canceled += ActionStopMoveCanceled;
            _pushFast.started += ActionFastPerformed;
            _pushFast.canceled += ActionStopFastCanceled;
        }
        // 建物説明開始処理
        {
            _pushBuildEnter = GameManager.Instance._InputControls.Player.BuildEnter;
            _pushBuildEnter.started += ActionBuildEnterStart;
        }
        return Disposable.Create(() =>
        {
            _pushMove.started -= ActionMovePerformed;
            _pushMove.canceled -= ActionStopVector2Canceled;
            _pushNormal.started -= ActionNormalImputPerformed;
            _pushNormal.canceled -= ActionStopMoveCanceled;
            _pushFast.started -= ActionFastPerformed;
            _pushFast.canceled -= ActionStopFastCanceled;
            _pushBuildEnter.started -= ActionBuildEnterStart;
        });
    }


    // キー入力
    /// 移動処理
    public void ActionMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        int moveParam = 1;
        const float NotMoveBoundaryParam = 10.0f;
        const float NormalMoveBoundaryParam = 100.0f;

        /*
        if (!m_isNormalMove  && !m_isFastMove &&
            -NotMoveBoundaryParam < moveInput.x && moveInput.x < NotMoveBoundaryParam)
        {
            moveParam = 0;
        }
        else 
        */
        if (!m_isFastMove  &&
            -NormalMoveBoundaryParam < moveInput.x && moveInput.x < NormalMoveBoundaryParam)
        {
            moveParam = 1;
            m_isNormalMove = true;
        }
        else
        {
            // 早歩き
            moveParam = 2;
            m_isFastMove = true;
        }

        if (moveInput.x > 0)
        {
            // スプライトを通常の向きで表示
            moveInput.x = moveParam;
            spriteRenderer.flipX = false;
        }
        else
        {
            // スプライトを通常の逆向きで表示
            moveInput.x = moveParam  * - 1;
            spriteRenderer.flipX = true;
        }
    }
    public void ActionNormalImputPerformed(InputAction.CallbackContext context)
    {
        // Walk
        this.GetComponent<Animator>().SetBool("IsWalk", true);
        m_isMove = true;
    }
    public void ActionFastPerformed(InputAction.CallbackContext context)
    {
        // Walk
        this.GetComponent<Animator>().SetBool("IsWalk", true);
        m_isFastMove = true;
    }
    public void ActionStopVector2Canceled(InputAction.CallbackContext context)
    {
        // Wait
        //moveInput = Vector2.zero;
        //moveInput.x = 0;
    }
    public void ActionStopMoveCanceled(InputAction.CallbackContext context)
    {
        // Wait
        this.GetComponent<Animator>().SetBool("IsWalk", false);
        moveInput = Vector2.zero;
        moveInput.x = 0;
        m_isMove = false;
        m_isFastMove = false;
        m_isNormalMove = false;
    }
    public void ActionStopFastCanceled(InputAction.CallbackContext context)
    {
        m_isFastMove = false;
    }
    /// 建物説明処理
    public async void ActionBuildEnterStart(InputAction.CallbackContext context)
    {
        if (m_isInBuilding)
        {
            if (m_inBuildingInfo != null)
            {
                // SE
                GameManager.Instance.m_ResidentFlow.m_soundManager.PlaySE(SoundManager.SE_TYPE.SELECT);

                string storyCSVName = m_inBuildingInfo.storyFileName;
                await m_scene.StartTextWindow(storyCSVName, m_token);
                // 終了
                m_inBuildingInfo.m_isFinishDisp = true;
            }
            else if(m_nextSceneStr.Length != 0)
            {
                // シーン遷移
                await GameManager.Instance.m_ResidentFlow.GotoNextScene(m_nextSceneStr);
            }
            ResetBuildInfo();
        }
    }

    // 衝突判定
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Building"))
        {
            // 建物の場合
            m_inBuildingInfo = other.gameObject.GetComponent<Building>();
            if (!m_inBuildingInfo.m_isFinishDisp)
            {
                // まだ見ていなければ
                m_isInBuilding = true;
            }
        }
        else if(other.gameObject.tag.Equals("SceneMoveObj"))
        {
            // scene移動オブジェクト
            m_isInBuilding = true;
            m_nextSceneStr = other.gameObject.GetComponent<SceneMoveObj>().m_sceneName;
        }
        else if (other.gameObject.tag.Equals("Goal"))
        {
            // ゴールオブジェクト
            if (!m_isGoal)
            {
                //触れた瞬間ゴール演出を開始する
                m_isGoal = true;
                StartGoalStory().Forget();
            }

        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Building")
            || other.gameObject.tag.Equals("SceneMoveObj")
            || other.gameObject.tag.Equals("Goal"))
        {
            // 建物の場合は情報リセット
            ResetBuildInfo();
        }
    }
    void ResetBuildInfo()
    {
        m_isInBuilding = false;
        m_inBuildingInfo = null;
        m_nextSceneStr = "";
    }

    // 未確認ストーリーの検知時ビックリマーク
    private void DispMark(bool isDisp)
    {
        m_Manpu.SetActive(isDisp);
    }

    private async UniTask StartGoalStory()
    {

        string storyCSVName = "StoryGoal.csv";
        await m_scene.StartTextWindow(storyCSVName, m_token);

        // BGMを止める
        await GameManager.Instance.m_ResidentFlow.m_soundManager.StopBGM(true);

        // 終了したらホワイトアウトでシーン遷移
        await GameManager.Instance.m_ResidentFlow.GotoNextScene("End", GameUtility.LoadFadeType.WhiteLoad);
    }
}
