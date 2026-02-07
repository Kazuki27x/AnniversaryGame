using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class ActorController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    // キーアクション
    private InputAction _pushMove;
    private Vector2 moveInput;


    private void Start()
    {

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
        moveInput = context.ReadValue<Vector2>();
    }
    public void ActionStopCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }
}
