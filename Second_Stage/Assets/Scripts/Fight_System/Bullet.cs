using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    public float speed = 375f;
    public int damage = 1;
    public Rigidbody2D rb;
    // Use this for initialization
    void Start() {
        if (Input.GetAxis("Vertical") == 0)
        {
            rb.velocity = transform.right * speed;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            rb.velocity = transform.up * speed;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            rb.velocity = -transform.up * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        if (hitInfo.CompareTag("Ground") || hitInfo.CompareTag("Player") || hitInfo.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
