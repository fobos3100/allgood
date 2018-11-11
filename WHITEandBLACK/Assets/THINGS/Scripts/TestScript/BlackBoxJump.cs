using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoxJump : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isGrounded = true;
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
        if (Input.GetButtonDown("Jump"))
        {          
                rb.velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().AddForce(jumpForce, ForceMode2D.Impulse);
        }
    }
}
