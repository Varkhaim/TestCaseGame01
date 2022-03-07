using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float HorizontalSpeed = 2.0f;
    public float JumpStrength = 5f;
    public int MaxDoubleJumps = 1;

    [Header("Params")]
    public LayerMask SolidLayer;
    public float GroundedDistance = 0.1f;
    public float MapBottomLimit = -5f;

    [Header("Attack")]
    public Vector2 ShootOffset;
    public GameObject BulletPrefab;
    public float BulletSpeed = 1f;

    [Header("Events")]
    public UnityEvent OnDeath;

    private Rigidbody2D rbody;
    private CapsuleCollider2D capsule;

    private bool isGrounded = true;
    private int doubleJumps = 1;
    private int facingDirection = 1;
    private Lift CurrentLift;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        CheckGround();
        HandleInput();
        CheckBounds();
    }

    private void CheckBounds()
    {
        if (transform.position.y < MapBottomLimit)
        {
            OnDeath.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void HandleInput()
    {
        if (MoveLeftPressed())
            TryMoveLeft();
        else
        if (MoveRightPressed())
            TryMoveRight();
        else
            StopHorizontalMovement();

        if (JumpPressed())
            TryJump();
        if (ShootPressed())
            Shoot();

    }

    private void StopHorizontalMovement()
    {
        rbody.velocity = new Vector2(0, rbody.velocity.y);
    }

    private bool MoveLeftPressed()
    {
        if (Gamepad.current != null)
            return Gamepad.current.leftStick.left.isPressed;
        return Keyboard.current.aKey.isPressed;
    }

    private bool MoveRightPressed()
    {
        if (Gamepad.current != null)
            return Gamepad.current.leftStick.right.isPressed;
        return Keyboard.current.dKey.isPressed;
    }

    private bool JumpPressed()
    {
        if (Gamepad.current != null)
            return Gamepad.current.squareButton.wasPressedThisFrame;
        return Keyboard.current.spaceKey.wasPressedThisFrame;
    }

    private bool ShootPressed()
    {
        if (Gamepad.current != null)
            return Gamepad.current.crossButton.wasPressedThisFrame;
        return Keyboard.current.fKey.wasPressedThisFrame;
    }

    private void TryMoveLeft()
    {
        if (!Physics2D.CapsuleCast(transform.position, capsule.size, capsule.direction, 0f, Vector2.left, 0.1f, SolidLayer))
            rbody.velocity = new Vector2(-HorizontalSpeed, rbody.velocity.y);
        facingDirection = -1;
    }

    private void TryMoveRight()
    {
        if (!Physics2D.CapsuleCast(transform.position, capsule.size, capsule.direction, 0f, Vector2.right, 0.1f, SolidLayer))
            rbody.velocity = new Vector2(HorizontalSpeed, rbody.velocity.y);
        facingDirection = 1;
    }

    private void TryJump()
    {
        if (isGrounded)
        {
            Jump();
            return;
        }
        if (CanDoubleJump())
        {
            DoubleJump();
        }
    }

    private void Jump()
    {
        rbody.velocity = new Vector2(rbody.velocity.x, JumpStrength);
    }

    private void DoubleJump()
    {
        doubleJumps -= 1;
        Jump();
    }

    private bool CanDoubleJump()
    {
        return (doubleJumps > 0);
    }

    private void CheckGround()
    {
        RaycastHit2D beneath = Physics2D.CapsuleCast(transform.position, capsule.size, capsule.direction, 0f, Vector2.down, GroundedDistance, SolidLayer);
        if (beneath)
        {
            TryFindLift(beneath);
            SetGrounded();
            return;
        }

        isGrounded = false;
    }

    private void SetGrounded()
    {
        if (isGrounded) return;
        doubleJumps = MaxDoubleJumps;
        isGrounded = true;
    }

    public void Shoot()
    {
        GameObject spawnedBullet = Instantiate(BulletPrefab, transform.position + new Vector3(facingDirection * ShootOffset.x, ShootOffset.y), Quaternion.identity);
        Bullet bullet = spawnedBullet.GetComponent<Bullet>();

        bullet.SetupBullet(capsule, new Vector3(facingDirection, 0f), BulletSpeed);
    }

    private void TryFindLift(RaycastHit2D beneath)
    {
        Lift lift = beneath.collider.gameObject.GetComponent<Lift>();
        if (!lift)
        {
            if (CurrentLift)
                CurrentLift.OnMovement.RemoveListener(MoveVerticaly);
            CurrentLift = null;
            return;
        }

        if (CurrentLift == lift) return;

        CurrentLift = lift;
        CurrentLift.OnMovement.AddListener(MoveVerticaly);
    }

    private void MoveVerticaly(float value)
    {
        transform.Translate(new Vector3(0f, value));
    }
}
