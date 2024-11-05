using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
public class PlayerController : MonoBehaviour
{
    TouchingDirection touchingDirection;
    Rigidbody2D Rigidbody;
    Animator animator;

    Vector2 MoveInput;

    public float Flyspeed = 5f;
    public float Jump = 10f;

    public float WalkSpeed = 5f;
    public float RunSpeed = 7f;

    [SerializeField]
    private bool _IsMoving = false;

    public bool IsMoving
    {
        get
        {
            return _IsMoving;
        }
        private set
        {
            _IsMoving = value;
            animator.SetBool(AnimationStrings.IsMoving, value);

        }
    }
    [SerializeField]
    private bool _IsRunning = false;
    public bool IsRunning
    {
        get
        {
            return _IsRunning;
        }
        private set
        {
            _IsRunning = value;
            animator.SetBool(AnimationStrings.IsRunning, value);
        }
    }
    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.CanMove); }
    }
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirection.IsOnWall)
                {
                    if (touchingDirection.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return RunSpeed;
                        }
                        else
                        {
                            return WalkSpeed;
                        }
                    }
                    else
                    {
                        return Flyspeed;
                    }

                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }
    }
    private bool _IsfacingRight = true;
    public bool IsFacingRight
    {
        get { return _IsfacingRight; }
        private set
        {
            if (_IsfacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _IsfacingRight = value;
        }
    }
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_run"))
        {
            Debug.Log("Running");
        }
    }




    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
    }

    private void FixedUpdate()
    {
        Rigidbody.velocity = new Vector2(MoveInput.x * CurrentMoveSpeed, Rigidbody.velocity.y);
        animator.SetFloat(AnimationStrings.Y_Velocity, Rigidbody.velocity.y);
    }
    public void Onmove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        IsMoving = MoveInput != Vector2.zero;
        IsFacingDirection(MoveInput);
        Debug.Log("MoveInput: " + MoveInput);
        Debug.Log("IsMoving: " + IsMoving);
    }

    private void IsFacingDirection(Vector2 moveInput)
    {
        if (MoveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (MoveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void Onrun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.Jump);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Jump);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.Attack);

        }
    }
}
