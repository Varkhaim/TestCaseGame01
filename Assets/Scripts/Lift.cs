using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lift : MonoBehaviour
{
    [Header("Params")]
    public float Speed = 1f;
    public float TimeToWait = 2f;
    public Transform TopLimit;
    public Transform BottomLimit;

    private int movementDirection = -1;
    private float waitTimer = 0f;
    private bool isStopped = false;

    [Header("Movement Events")]
    public UnityFloatEvent OnMovement = new UnityFloatEvent();

    private void Update()
    {
        HandleMovement();
        HandleWait();
    }

    private void HandleWait()
    {
        if (!isStopped) return;

        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0f)
        {
            isStopped = false;
            movementDirection = -movementDirection;
        }
    }

    private void HandleMovement()
    {
        if (isStopped) return;

        float yOffset = movementDirection * Speed;
        transform.Translate(new Vector3(0f, yOffset));
        OnMovement.Invoke(yOffset);
        if ((movementDirection < 0) && (transform.position.y < BottomLimit.position.y))
        {
            transform.position = new Vector3(transform.position.x, BottomLimit.position.y);
            StopLift();
        }
        if ((movementDirection > 0) && (transform.position.y > TopLimit.position.y))
        {
            transform.position = new Vector3(transform.position.x, TopLimit.position.y);
            StopLift();
        }
    }

    private void StopLift()
    {
        isStopped = true;
        waitTimer = TimeToWait;
    }
}
