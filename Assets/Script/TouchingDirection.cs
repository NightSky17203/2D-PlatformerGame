using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    public ContactFilter2D castFilter;

    public float groundDistance = 0.05f;

    public float WallDistance = 0.2f;

    public float CeilingDistance = 0.05f;

    CapsuleCollider2D touchingCol;
    Animator animator;

    RaycastHit2D[] Groundhits = new RaycastHit2D[5];

    RaycastHit2D[] Wallhits = new RaycastHit2D[5];

    RaycastHit2D[] Ceilinghits = new RaycastHit2D[5];

    [SerializeField]
    private bool _IsGrounded;

    public bool IsGrounded
    {
        get { return _IsGrounded; }
        private set
        {
            _IsGrounded = value;
            animator.SetBool(AnimationStrings.IsGrounded, value);
        }
    }
    private bool _IsOnWall;


    public bool IsOnWall
    {
        get { return _IsOnWall; }
        private set
        {
            _IsOnWall = value;
            animator.SetBool(AnimationStrings.IsOnWall, value);
        }
    }
    private Vector2 WallDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    private bool _IsOnCeiling;

    public bool IsOnCeiling
    {
        get { return _IsOnCeiling; }
        private set
        {
            _IsOnCeiling = value;
            animator.SetBool(AnimationStrings.IsOnCeiling, value);
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        touchingCol = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, Groundhits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(WallDirection, castFilter, Wallhits, WallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, Ceilinghits, CeilingDistance) > 0;
    }
}
