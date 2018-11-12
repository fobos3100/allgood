using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float moveSpeed = 100f;
    private float maxSpeed = 150f;
    public static bool canMove;

    // Update is called once per frame
    void Update()
    {
        if (canMove == true)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            rb.AddForce(Vector2.right * moveSpeed * h);

            if (rb.velocity.x > maxSpeed)
            {
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            }

            if (rb.velocity.x < -maxSpeed)
            {
                rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
            }

            if (h > 0)
                sr.flipX = false;
            else if (h < 0)
                sr.flipX = true;
        }
    }
}