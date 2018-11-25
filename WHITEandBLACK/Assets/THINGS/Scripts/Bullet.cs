using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    public float speed = 500f;
    public int damage = 1;
    public Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb.velocity = transform.right * speed;
	}

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
