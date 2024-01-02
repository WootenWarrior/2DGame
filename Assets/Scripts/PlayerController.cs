using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public Vector2 force;
    public float dampingFactor;
    public Vector2 placeholderForce;
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.velocity = PlayerInput * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision);
        if(collision.collider.CompareTag("TestCollision")){
            force += placeholderForce;
            Destroy(collision.gameObject);
        }
    }
}
