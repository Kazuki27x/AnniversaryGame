using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class ActorController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private SpriteRenderer spriteRenderer;

    // キーアクション
    private InputAction _pushMove;
    private Vector2 moveInput;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // キー操作登録
        SetInputAction().AddTo(this);
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


        return Disposable.Create(() =>
        {
            _pushMove.started -= ActionMovePerformed;
            _pushMove.canceled -= ActionStopCanceled;
        });
    }

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
}
