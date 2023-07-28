using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _inputActions;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private float _movSpeed, _jumpForce;

    private float _direction;

    [SerializeField]
    private LayerMask _floorLayer;

    [SerializeField]
    private Vector2 _groundedPosition;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Jump.performed += Jump;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) return;
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void Update()
    {
        _direction = _inputActions.Player.Movement.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rb.velocity = new Vector2(_direction * _movSpeed * Time.fixedDeltaTime, _rb.velocity.y);
    }

    private bool IsGrounded()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast((Vector3)_groundedPosition + transform.position + Vector3.right * -0.15f, Vector2.down, 0.3f, _floorLayer);
        RaycastHit2D hitRight = Physics2D.Raycast((Vector3)_groundedPosition + transform.position + Vector3.right * 0.15f, Vector2.down, 0.3f, _floorLayer);

        return hitLeft.collider != null || hitRight.collider != null;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine((Vector3)_groundedPosition + transform.position + Vector3.right * 0.15f, (Vector3)_groundedPosition + transform.position + Vector3.right * 0.15f + Vector3.down * 0.3f, Color.blue);
        Debug.DrawLine((Vector3)_groundedPosition + transform.position + Vector3.right * -0.15f, (Vector3)_groundedPosition + transform.position + Vector3.right * -0.15f + Vector3.down * 0.3f, Color.blue);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.localScale.x > transform.localScale.x)
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Spike"))
        {
            GameManager.Instance.ReloadLevel();
        }
    }

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
        {
            CheckForWin();
        }
    }

    private void CheckForWin()
    {
        if(FindObjectsOfType<PlayerController>().Length <= 1){
            GameManager.Instance.LoadNextLevel();
        }
        else
        {
            print("Todavia no ganaste");
        }
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }
}
