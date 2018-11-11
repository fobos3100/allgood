using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCoolJump : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2f;
    private Vector2 jumpForce = new Vector2(0, 100f);

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        if (Input.GetButtonDown("Jump"))
        {
             rb.velocity = Vector2.zero;
             GetComponent<Rigidbody2D>().AddForce(jumpForce, ForceMode2D.Impulse);
        }
    }
}
