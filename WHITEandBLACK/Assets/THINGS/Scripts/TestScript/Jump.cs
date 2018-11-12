using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

    private Rigidbody2D rb;

    private float jumpForce = 150f;
    public bool isGrounded;
    public bool canDoubleJump;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    bool canMove = true;

    public Collider2D groundTrigger;

  

	// Update is called once per frame
	void Update () {
        if (canMove == true)
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
                if (isGrounded)
                {
                    //rb.velocity = Vector2.zero;
                    rb.velocity = Vector2.up * jumpForce;
                    isGrounded = false;
                }
                else if (canDoubleJump)
                {
                    canDoubleJump = false;
                    //rb.velocity = Vector2.zero;
                    rb.velocity = Vector2.up * jumpForce;
                }
            }
        }
    }
}
