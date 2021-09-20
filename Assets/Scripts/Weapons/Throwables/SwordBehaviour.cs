using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SwordBehaviour : NetworkBehaviour
{
    public float projectileVelocity;
    [SerializeField] Rigidbody2D rb;


    private void Start() {
        Destroy(gameObject,1f);
    }

    private void FixedUpdate() {
        rb.velocity = transform.up * 1.5f;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }

}
