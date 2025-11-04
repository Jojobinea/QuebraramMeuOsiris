using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.GridBrushBase;

public class RotateView : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 180f;

    // NEW: reference to the player to read IsGrounded
    [SerializeField] private PlayerController _player;

    private Transform _phaseTransform;
    private bool _isRotating = false;
    private int _rotationDirection = 0;
    private Quaternion _targetRotation;

    void Start()
    {
        EventManager.onReachedDoorEvent += CheckIfRightSideUp;
        _phaseTransform = gameObject.GetComponent<Transform>();
    }

    private void OnDestroy()
    {
        EventManager.onReachedDoorEvent -= CheckIfRightSideUp;
    }

    void Update()
    {
        if (!_isRotating)
        {
            // Gate rotation on grounded
            bool canRotate = (_player != null && _player.IsGrounded);

            if (canRotate)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                    StartRotation(1);
                else if (Input.GetKeyDown(KeyCode.E))
                    StartRotation(-1);
            }
        }
        else
        {
            RotatePhaseView();
        }
    }

    private void StartRotation(int dir)
    {
        if (_player != null && !_player.IsGrounded)
        {
            return;
        }

        _isRotating = true;
        _rotationDirection = dir;
        EventManager.OnPhaseIsRotatingTrigger();

        // Round current rotation to nearest 90°
        float currentZ = Mathf.Round(_phaseTransform.eulerAngles.z / 90f) * 90f;
        // Target is +90° or -90° from that
        float targetZ = currentZ + (90f * dir);

        _targetRotation = Quaternion.Euler(0f, 0f, targetZ);
    }

    private void RotatePhaseView()
    {
        _phaseTransform.rotation = Quaternion.RotateTowards(
            _phaseTransform.rotation,
            _targetRotation,
            _rotationSpeed * Time.deltaTime
        );

        if (Quaternion.Angle(_phaseTransform.rotation, _targetRotation) < 0.1f)
        {
            _phaseTransform.rotation = _targetRotation;
            _isRotating = false;
            _rotationDirection = 0;
            EventManager.OnPhaseFinishedRotatingTrigger();
        }
    }

    private void CheckIfRightSideUp()
    {
        if (Quaternion.Angle(_phaseTransform.rotation, Quaternion.Euler(0f, 0f, 0f)) < 0.1f && !_isRotating)
        {
            EventManager.OnGameWinTrigger();
        }
    }
}
