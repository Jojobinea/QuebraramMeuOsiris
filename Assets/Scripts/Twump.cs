using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Twump : MonoBehaviour
{
    [SerializeField] private Transform rotatingViewRoot;
    [SerializeField] private Transform spriteRoot;
    [SerializeField] private GameObject playerDetectionTrigger;

    [Header("Ground Stick (optional)")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float minGroundNormalY = 0.5f;
    [SerializeField] private bool makeStaticWhenStuck = true;

    [Header("Crush Settings")]
    [SerializeField] private float downEpsilon = 0.01f;
    [SerializeField] private float verticalBias = 0.05f;

    private Rigidbody2D _rb;
    private bool _isStuck = false;

    private void Start()
    {
        EventManager.OnPhaseIsRotatingEvent += DetectionOff;
        EventManager.onPhaseFinishedRotatingEvent += DetectionOn;

        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        EventManager.OnPhaseIsRotatingEvent -= DetectionOff;
        EventManager.onPhaseFinishedRotatingEvent -= DetectionOn;
    }

    private void LateUpdate()
    {
        if (!rotatingViewRoot || !spriteRoot) return;

        float counterZ = -rotatingViewRoot.eulerAngles.z;
        spriteRoot.localRotation = Quaternion.Euler(0f, 0f, counterZ);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryCrushPlayer(collision);

        if (collision.collider.CompareTag("BreakableWall") && collision.relativeVelocity.magnitude >= 0.5f)
        {
            Debug.Log("wall");
            Destroy(collision.collider.gameObject);
        }

        if (!_isStuck && IsGroundCollision(collision))
            StickToGround();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryCrushPlayer(collision);
    }

    private void TryCrushPlayer(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        bool movingDown = _rb.linearVelocity.y < -downEpsilon;
        float selfY = transform.position.y;
        float playerY = collision.collider.bounds.center.y;
        bool above = selfY > playerY + verticalBias;

        if (movingDown && above)
        {
            Debug.Log("[Twump] CRUSH");
            EventManager.OnGameLooseTrigger();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("twump: arm & drop");
            ArmAndDrop();
        }
    }

    private void DetectionOff()
    {
        playerDetectionTrigger.SetActive(false);
    }
    private void DetectionOn() 
    { 
        if(!_isStuck)
            playerDetectionTrigger.SetActive(true); 
    }

    private bool IsGroundCollision(Collision2D collision)
    {
        bool isGroundLayer = (groundLayer.value & (1 << collision.collider.gameObject.layer)) != 0;
        if (!isGroundLayer) return false;

        if (collision.contactCount > 0)
        {
            var n = collision.GetContact(0).normal;
            return n.y >= minGroundNormalY;
        }
        return true;
    }

    private void StickToGround()
    {
        _isStuck = true;
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        if (makeStaticWhenStuck) _rb.bodyType = RigidbodyType2D.Static;
        else _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        playerDetectionTrigger.SetActive(false);
        Debug.Log("[Twump] Stuck to ground");
    }

    private void ArmAndDrop()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        Debug.Log("[Twump] Dropping");
    }
}
