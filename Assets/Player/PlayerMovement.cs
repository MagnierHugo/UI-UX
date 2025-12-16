using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


public sealed class PlayerMovement : MonoBehaviour
{
    private new Rigidbody rigidbody;
    [SerializeField] private float speed = 5;
    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        InputActionMap inputActionMap = GetComponent<PlayerInput>().currentActionMap;
        moveAction = inputActionMap.FindAction("Move", true);
        jumpAction = inputActionMap.FindAction("Jump", true);
    }
    private void OnEnable()
    {
        moveAction.Enable();
        moveAction.performed += OnPlayerMove;
        moveAction.canceled += OnPlayerMove;

        jumpAction.Enable();
        jumpAction.performed += OnPlayerJump;
        jumpAction.canceled += OnPlayerCancelsJump;
    }
    private void OnDisable()
    {
        moveAction.performed -= OnPlayerMove;
        moveAction.canceled -= OnPlayerMove;
        moveAction.Disable();

        jumpAction.performed -= OnPlayerJump;
        jumpAction.canceled -= OnPlayerCancelsJump;
        jumpAction.Disable();

        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        moveInput = Vector2.zero;
    }


    private Vector2 moveInput;
    private void OnPlayerMove(InputAction.CallbackContext context)
        => moveInput = context.ReadValue<Vector2>();

    private void FixedUpdate()
    {
        HandleCollisions();

        Vector3 localVelocity = new Vector3(moveInput.x, 0, moveInput.y).normalized * speed;
        if (shouldJump)
        {
            localVelocity.y = jumpForce;
            isJumping = true;
        }
        else if (jumpCanceled)
        {
            localVelocity.y = rigidbody.linearVelocity.y * jumpCancelCoefficient;
            jumpCanceled = false;
        }
        else
        {
            localVelocity.y = rigidbody.linearVelocity.y;
        }

        if (isJumping)
            isJumping = rigidbody.linearVelocity.y > .0f; // reset jumping state as soon as we start falling back down

        shouldJump = false;

        rigidbody.linearVelocity = transform.TransformDirection(localVelocity);
    }

    private void Update() => HandleJump();

    #region Collisions
    private const float groundCheckRadius = .5f;
    [SerializeField] private Transform groundCheck;
    private bool isCollidingDown;
    private void HandleCollisions()
    {
        bool grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, Layers.Default, QueryTriggerInteraction.Ignore);
        if (isCollidingDown != grounded)
            if (isCollidingDown)  // just jumped
                timeLeftGround = Time.time;
            else  // just landed
                coyoteUsable = true;

        isCollidingDown = grounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    #endregion

    #region Jump
    [SerializeField] private float jumpForce;
    private readonly float coyoteTimeThreshold = .1f;
    private bool coyoteUsable;
    private float timeLeftGround;
    private bool CanUseCoyote => coyoteUsable && !isCollidingDown && timeLeftGround + coyoteTimeThreshold > Time.time;
    private bool HasBufferedJump => lastJumpPressed + jumpBuffer > Time.time;
    private const float jumpBuffer = .2f;

    private float lastJumpPressed = float.NegativeInfinity;
    

    private bool isJumping;
    private bool shouldJump;

    private void HandleJump()
    {
        if (isJumping)
            return;

        if (HasBufferedJump)
            shouldJump |= (CanUseCoyote || isCollidingDown) && !jumpCanceled;
    }

    private void OnPlayerJump(InputAction.CallbackContext context)
        => lastJumpPressed = Time.time;

    [SerializeField][Range(0, 1)] private float jumpCancelCoefficient = .75f;
    private bool jumpCanceled;
    private void OnPlayerCancelsJump(InputAction.CallbackContext context)
        => jumpCanceled = isJumping; // if jump wasn t initiated then avoid canceling it 
    #endregion
}