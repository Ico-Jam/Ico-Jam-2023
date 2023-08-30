using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerInputActions _inputActions;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private float _movSpeed, _jumpForce, _groundCheckDistance;

    private float _direction;

    [SerializeField]
    private LayerMask _floorLayer;

    [SerializeField]
    private Vector2 _groundedPosition;

    [SerializeField]
    private Animator _anim;

    [SerializeField]
    private SpriteRenderer _srUp, _srDown;

    [SerializeField]
    private Sprite _brokenSprite;

    private bool _isMerging;

    [SerializeField]
    private AudioSource _jumpSound, _collideSound, _deathSound, _mergeSound;

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
        _jumpSound.Play();
    }

    private void Update()
    {
        _direction = _inputActions.Player.Movement.ReadValue<float>();
        _anim.SetBool("Walking", _rb.velocity.x != 0 && _rb.velocity.y == 0);
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
        RaycastHit2D hitLeft = Physics2D.Raycast((Vector3)_groundedPosition + transform.position + Vector3.right * -_groundCheckDistance, Vector2.down, 0.3f, _floorLayer);
        RaycastHit2D hitRight = Physics2D.Raycast((Vector3)_groundedPosition + transform.position + Vector3.right * _groundCheckDistance, Vector2.down, 0.3f, _floorLayer);
        RaycastHit2D hitCenter = Physics2D.Raycast((Vector3)_groundedPosition + transform.position, Vector2.down, 0.3f, _floorLayer);

        return hitLeft.collider != null || hitRight.collider != null || hitCenter.collider != null;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine((Vector3)_groundedPosition + transform.position + Vector3.right * _groundCheckDistance, (Vector3)_groundedPosition + transform.position + Vector3.right * _groundCheckDistance + Vector3.down * 0.3f, Color.blue);
        Debug.DrawLine((Vector3)_groundedPosition + transform.position + Vector3.right * -_groundCheckDistance, (Vector3)_groundedPosition + transform.position + Vector3.right * -_groundCheckDistance + Vector3.down * 0.3f, Color.blue);
        Debug.DrawLine((Vector3)_groundedPosition + transform.position, (Vector3)_groundedPosition + transform.position + Vector3.down * 0.3f, Color.blue);
    }

    public void Merge(Transform position)
    {
        if (_isMerging) return;
        _isMerging = true;
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders) collider.enabled = false;
        _rb.gravityScale = 0;
        _rb.velocity = Vector2.zero;
        LeanTween.move(gameObject, position.position, 0.25f).setOnComplete(() =>
        {
            Invoke(nameof(ActiveFalse), 0.25f);
        });
    }

    private void ActiveFalse()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.localScale.x < transform.localScale.x)
        {
            StartCoroutine(GameManager.Instance.Merge(collision.transform.GetComponent<PlayerController>(), transform));
            StartCoroutine(FreezeMerge());
            _mergeSound.Play();
        }
        if (collision.gameObject.CompareTag("Spike"))
        {
            _srUp.enabled = false;
            _srDown.sprite = _brokenSprite;
            _inputActions.Player.Disable();
            _deathSound.Play();
            _rb.gravityScale = 0;
            _rb.velocity = Vector2.zero;
            GameManager.Instance.Invoke(nameof(GameManager.Instance.ReloadLevel), 0.5f);
        }
        if ((_floorLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            if(Time.timeSinceLevelLoad > 0.25f)
            {
                _collideSound.Play();
            }
        }
    }

    private IEnumerator FreezeMerge()
    {
        _rb.gravityScale = 0;
        _rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.75f);
        _rb.gravityScale = 3;
    }

    private void CheckForWin()
    {
        if(FindObjectsOfType<PlayerController>().Length <= 1){
            GameManager.Instance.Invoke(nameof(GameManager.Instance.LoadNextLevel), 0.25f);
        }
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        if (gameObject.scene.isLoaded)
        {
            CheckForWin();
        }
        _inputActions.Player.Disable();
        _inputActions.Player.Jump.performed -= Jump;
    }
}
