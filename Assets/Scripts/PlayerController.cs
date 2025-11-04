using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _playerSpeed = 5f;
    [SerializeField] private Transform rotatingViewRoot;
    [SerializeField] private Transform spriteRoot;

    // ==== Ground Check (agora em caixa) ====
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;                 // ponto nos p�s
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.6f, 0.1f); // largura/altura da caixa
    [SerializeField] private float groundCheckAngle = 0f;           // rota��o da caixa (geralmente 0)
    [SerializeField] private LayerMask groundLayer;                 // defina para a layer "Ground" no Inspector

    private Rigidbody2D _rb;
    private Collider2D _collider;
    private bool _canMove = true;
    private bool _isGrounded;

    // Flip cache
    private float _spriteBaseScaleX = 1f;

    // Public read-only accessor
    public bool IsGrounded => _isGrounded;

    void Start()
    {
        EventManager.OnPhaseIsRotatingEvent += FreezePlayer;
        EventManager.onPhaseFinishedRotatingEvent += UnfreezePlayer;

        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        if (spriteRoot == null) spriteRoot = transform; // fallback
        if (groundCheck == null) groundCheck = transform; // fallback

        _spriteBaseScaleX = Mathf.Abs(spriteRoot.localScale.x);

        UnfreezePlayer();
    }

    private void OnDestroy()
    {
        EventManager.OnPhaseIsRotatingEvent -= FreezePlayer;
        EventManager.onPhaseFinishedRotatingEvent -= UnfreezePlayer;
    }

    void Update()
    {
        CheckGrounded();

        if (_canMove)
            MovePlayer();
    }

    private void LateUpdate()
    {
        if (rotatingViewRoot == null || spriteRoot == null) return;

        float counterZ = -rotatingViewRoot.eulerAngles.z;
        spriteRoot.localRotation = Quaternion.Euler(0f, 0f, counterZ);
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        Vector2 vel = _rb.linearVelocity;

        float horizontalSpeed = _playerSpeed;
        if (_rb.linearVelocity.y < -0.01f) // caindo
        {
            horizontalSpeed *= 0.5f;
        }

        vel.x = moveX * horizontalSpeed;
        _rb.linearVelocity = vel;

        if (moveX != 0f)
        {
            float dir = Mathf.Sign(moveX);
            Vector3 s = spriteRoot.localScale;
            s.x = _spriteBaseScaleX * dir;
            spriteRoot.localScale = s;
        }
    }

    private void FreezePlayer()
    {
        Debug.Log("FreezePlayer");
        _canMove = false;
        _rb.linearVelocity = Vector2.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void UnfreezePlayer()
    {
        Debug.Log("Unfreeze player");
        _canMove = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation; // evita girar, mas permite mover
        _rb.linearVelocity = Vector2.zero;
    }

    public bool CheckGrounded()
    {
        // Centro da caixa = posi��o do groundCheck
        Vector2 center = groundCheck != null ? (Vector2)groundCheck.position : (Vector2)transform.position;

        // OverlapBox checa se a caixa toca em qualquer colisor na layer de Ground
        _isGrounded = Physics2D.OverlapBox(center, groundCheckSize, groundCheckAngle, groundLayer) != null;

        return _isGrounded;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;

            // Desenha a caixa de ground check
            Matrix4x4 old = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(groundCheck.position, Quaternion.Euler(0, 0, groundCheckAngle), Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(groundCheckSize.x, groundCheckSize.y, 0.01f));
            Gizmos.matrix = old;
        }
    }
}
