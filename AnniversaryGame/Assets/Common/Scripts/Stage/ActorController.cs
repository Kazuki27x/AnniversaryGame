using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Threading;
using UniRx;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    [SerializeField] private BaseScene m_scene;
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject m_Manpu;

    private SpriteRenderer spriteRenderer;

    // キーアクション
    /// 移動処理
    private InputAction _pushMove;
    private Vector2 moveInput;
    /// 建物説明処理
    private InputAction _pushBuildEnter;

    // 衝突情報
    private bool m_isInBuilding = false; // 建物内にいるか
    private Building m_inBuildingInfo;

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

    private void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, 0);
        GetComponent<Rigidbody2D>().MovePosition(this.transform.position + move * speed * Time.fixedDeltaTime);
    }

    private System.IDisposable SetInputAction()
    {
        // 移動処理
        {
            _pushMove = GameManager.Instance._InputControls.Player.Move;
            _pushMove.started += ActionMovePerformed;
            _pushMove.canceled += ActionStopCanceled;
        }
        // 建物説明開始処理
        {
            _pushBuildEnter = GameManager.Instance._InputControls.Player.BuildEnter;
            _pushBuildEnter.started += ActionBuildEnterStart;
        }
        return Disposable.Create(() =>
        {
            _pushMove.started -= ActionMovePerformed;
            _pushMove.canceled -= ActionStopCanceled;
            _pushBuildEnter.started -= ActionBuildEnterStart;
        });
    }


    // キー入力
    /// 移動処理
    public void ActionMovePerformed(InputAction.CallbackContext context)
    {
        // Walk
        this.GetComponent<Animator>().SetBool("IsWalk", true);

        moveInput = context.ReadValue<Vector2>();
        if (moveInput.x > 0)
        {
            // スプライトを通常の向きで表示
            spriteRenderer.flipX = false;
        }
        else
        {
            // スプライトを通常の逆向きで表示
            spriteRenderer.flipX = true;
        }
    }
    public void ActionStopCanceled(InputAction.CallbackContext context)
    {
        // Wait
        this.GetComponent<Animator>().SetBool("IsWalk", false);
        moveInput = Vector2.zero;
    }
    /// 建物説明処理
    public async void ActionBuildEnterStart(InputAction.CallbackContext context)
    {
        if (m_isInBuilding && m_inBuildingInfo != null)
        {
            string storyCSVName = m_inBuildingInfo.storyFileName;
            List<TextContentData> tmpList = new List<TextContentData>();
            await m_scene.StartTextWindow(storyCSVName, m_token);
            // 終了
            m_inBuildingInfo.m_isFinishDisp = true;
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
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Building"))
        {
            // 建物の場合は情報リセット
            ResetBuildInfo();
        }
    }
    void ResetBuildInfo()
    {
        m_isInBuilding = false;
        m_inBuildingInfo = null;
    }

    // 未確認ストーリーの検知時ビックリマーク
    private void DispMark(bool isDisp)
    {
        m_Manpu.SetActive(isDisp);
    }
}
