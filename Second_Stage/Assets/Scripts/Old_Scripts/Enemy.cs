using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public int health = 5;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

        void Die()
        {
            Destroy(gameObject);
        }

    public IEnumerator Knockback(float knockDur, float knockbackPwr, Vector2 knockbackDir)
    {
        float timer = 0;

        while (knockDur > timer)
        {
            timer += Time.deltaTime;

            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector3(knockbackDir.x * 0, knockbackDir.y * knockbackPwr, transform.position.z));

        }

        yield return 0;
    }

}
