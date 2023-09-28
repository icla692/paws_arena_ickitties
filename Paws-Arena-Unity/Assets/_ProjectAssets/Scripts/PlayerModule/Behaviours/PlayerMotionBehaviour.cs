using Anura;
using Anura.ConfigurationModule.Managers;
using System;
using UnityEngine;

public class PlayerMotionBehaviour : MonoBehaviour
{
    [SerializeField] private Collider2D ceilingCollider;

    public bool isPaused = false;
    private PlayerState playerState;

    private Transform _transform;
    private Rigidbody2D _rigidbody2D;

    private Vector3 velocity = Vector3.zero;


    private float lastJumpTime = 0;


    private void Awake()
    {
        _transform = transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void RegisterPlayerState(PlayerState state)
    {
        playerState = state;
    }

    public void RegisterMovementCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Movement.performed += OnMovementPerformed;
        playerActions.Movement.canceled += OnMovementCanceled;
    }
    public void RegisterMovementCallbacks(BotInputActions.PlayerActions playerActions)
    {
        playerActions.Movement.performed += OnMovementPerformed;
        playerActions.Movement.canceled += OnMovementCanceled;
    }

    internal void RegisterBotAPICallbacks(BotPlayerAPI botPlayerAPI)
    {
        botPlayerAPI.onMove += OnMovementPerformed;
        botPlayerAPI.onMoveCancelled += OnMovementCancelled;
        botPlayerAPI.onJumpStarted += OnJumpPerformed;
    }

    public void RegisterJumpCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Jump.started += OnJumpPerformed;
    }
    public void RegisterJumpCallbacks(BotInputActions.PlayerActions playerActions)
    {
        playerActions.Jump.started += OnJumpPerformed;
    }
    private void OnJumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext value)
    {
        OnJumpPerformed();
    }

    private void OnJumpPerformed()
    {
        if (isPaused) return;
        //Throttle
        if (playerState.hasJump || (Time.time - lastJumpTime < 1f)) return;

        lastJumpTime = Time.time;
        playerState.SetHasJump(true);
    }

    private void OnMovementPerformed(UnityEngine.InputSystem.InputAction.CallbackContext value)
    {
        OnMovementPerformed(value.ReadValue<float>());
    }

    private void OnMovementPerformed(float direction)
    {
        if (isPaused) return;
        SetMovementDirection(direction);
    }

    private void OnMovementCanceled(UnityEngine.InputSystem.InputAction.CallbackContext value)
    {
        OnMovementCancelled();
    }
    private void OnMovementCancelled()
    {
        if (isPaused) return;
        SetMovementDirection(0);
    }

    private void Update()
    {
        TryApplyRotationCorrection();
    }

    private void FixedUpdate()
    {
        if (playerState == null) return;
        if (playerState.movementDirection == 0 && !playerState.hasJump)
            return;

        if(playerState.isMidJump && CheckIfIsGrounded())
        {
            playerState.SetIsMidJump(false);
        }

        Move(playerState.movementDirection * Time.deltaTime * GetSpeed(), playerState.hasJumpImpulseQueued);
    }

    public void Move(float move, bool jumpQueued)
    {
        if (CheckIfIsGrounded() || GetAirControl())
        {
            var targetVelocity = new Vector2(move * 10f, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref velocity, GetMovementSmoothing());
        }

        if (CheckIfIsGrounded() && jumpQueued)
        {
            _rigidbody2D.AddForce(Vector2.up * GetJumpForce(), ForceMode2D.Impulse);
            playerState.SetQueueJumpImpulse(false);
        }else if(jumpQueued) //Jumped in-air. Wasted.
        {
            playerState.SetQueueJumpImpulse(false);
        }
    }

    private void SetMovementDirection(float value)
    {
        playerState.SetMovementDirection(value);
    }

    private float GetSpeed()
    {
        return ConfigurationManager.Instance.Config.GetPlayerSpeed();
    }

    private bool CheckIfIsGrounded()
    {
        return Physics2D.IsTouchingLayers(ceilingCollider);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"{collision.contacts[0].relativeVelocity.x} {collision.contacts[0].relativeVelocity.y}");
    }

    private bool GetAirControl()
    {
        return ConfigurationManager.Instance.Config.GetAirControl();
    }
    private float GetMovementSmoothing()
    {
        return ConfigurationManager.Instance.Config.GetMovementSmoothing();
    }

    private float GetJumpForce()
    {
        return ConfigurationManager.Instance.Config.GetPlayerJumpForce();
    }

    private void TryApplyRotationCorrection()
    {
        if (!_transform.eulerAngles.z.IsBetween(-30, 30))
        {
            var euler = _transform.eulerAngles;
            if (euler.z > 180)
                euler.z = euler.z - 360;

            euler.z = Mathf.Clamp(euler.z, -30, 30);

            _transform.eulerAngles = euler;
        }
    }

    public void SetIsPaused(bool val)
    {
        isPaused = val;
        if (isPaused)
        {
            playerState.SetMovementDirection(0);
        }
    }
}
