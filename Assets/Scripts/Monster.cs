using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{
    [Header("Movement")]
    public float MovementSpeed = 2f;
    public float AbyssCheckDistance = 1f;
    public float WallCheckDistance = 1f;
    public float GroundedDistance = 1f;
    public LayerMask SolidMask;
    public UnityEvent OnDeath;

    private float xValue;
    private Rigidbody2D rbody;
    private CapsuleCollider2D capsule;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        RandomizeMovementDirection();
    }

    private void RandomizeMovementDirection()
    {
        int random = UnityEngine.Random.Range(0, 1);
        xValue = random == 0 ? 1 : -1;
    }

    public void Kill()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        rbody.velocity = new Vector2(xValue * MovementSpeed, rbody.velocity.y);
        if (CheckForWall() || CheckForAbyss())
        {
            ReverseMovement();
        }
    }

    private bool CheckForAbyss()
    {
        if (!Physics2D.CapsuleCast(transform.position, capsule.size, capsule.direction, 0f, new Vector2(xValue, -0.25f), AbyssCheckDistance, SolidMask) && CheckGround())
        {
            return true;
        }
        return false;
    }

    private bool CheckForWall()
    {
        if (Physics2D.CapsuleCast(transform.position, capsule.size, capsule.direction, 0f, new Vector2(xValue, 0.1f), WallCheckDistance, SolidMask))
        {
            return true;
        }
        return false;
    }

    private bool CheckGround()
    {
        if (Physics2D.CapsuleCast(transform.position, capsule.size, capsule.direction, 0f, Vector2.down, GroundedDistance, SolidMask))
        {
            return true;
        }
        return false;
    }

    private void ReverseMovement()
    {
        xValue = -xValue;
    }
}
