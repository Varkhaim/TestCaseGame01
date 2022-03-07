using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rbody;
    private Collider2D col;
    private float currentLifetime;
    public float Lifetime = 3f;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        currentLifetime = Lifetime;
    }

    public void SetupBullet(Collider2D ownerCollider, Vector3 direction, float force)
    {
        Physics2D.IgnoreCollision(col, ownerCollider);
        rbody.velocity = direction * force;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Monster monster = collision.gameObject.GetComponent<Monster>();
        if (monster)
            monster.Kill();
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleLifetime();
    }

    private void HandleLifetime()
    {
        currentLifetime -= Time.deltaTime;
        if (currentLifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
