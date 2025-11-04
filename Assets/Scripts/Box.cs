using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Box : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Collider2D _col;
    [SerializeField] private Grid grid;

    [Header("Crush Settings")]
    [Tooltip("How little downward speed still counts as 'moving down'")]
    [SerializeField] private float downEpsilon = 0.01f;
    [Tooltip("How much higher than the player this object must be to count as 'above'")]
    [SerializeField] private float verticalBias = 0.05f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        EventManager.OnPhaseIsRotatingEvent += FreezeBox;
        EventManager.onPhaseFinishedRotatingEvent += UnfreezeBox;
    }

    private void OnDestroy()
    {
        EventManager.OnPhaseIsRotatingEvent -= FreezeBox;
        EventManager.onPhaseFinishedRotatingEvent -= UnfreezeBox;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryCrushPlayer(collision);
    }

    private void TryCrushPlayer(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        // must be moving downward (any amount)
        bool movingDown = _rb.linearVelocity.y < -downEpsilon;

        // must be above the player's center
        float selfY = transform.position.y;
        float playerY = collision.collider.bounds.center.y;
        bool above = selfY > playerY + verticalBias;

        if (movingDown && above)
        {
            Debug.Log("[Box] CRUSH");
            EventManager.OnGameLooseTrigger();
        }
    }

    public void SnapToGrid()
    {
        Vector3Int cell = grid.WorldToCell(transform.position);
        Vector3 center = grid.GetCellCenterWorld(cell);
        _rb.position = center;
        _rb.linearVelocity = Vector2.zero;
    }

    private void FreezeBox()
    {
        SnapToGrid();
        _rb.bodyType = RigidbodyType2D.Static;
        SnapToGrid();
    }
    private void UnfreezeBox()
    {
        SnapToGrid();
        _rb.bodyType = RigidbodyType2D.Dynamic;
        SnapToGrid();
    }
}
