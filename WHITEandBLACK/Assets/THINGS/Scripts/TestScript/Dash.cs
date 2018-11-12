using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {

    private Rigidbody2D rb;

    private float dashSpeed = 200f;
    private float startDashTime = 0.1f;
    private int direction;
    private float dashTime;
    private float dashCoolDown = 1f;
    private float nextDashTime = 0f;

    bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
    }

    void Update()
    {
        if (direction == 0)
        {
            if (Time.time > nextDashTime)
            {
                if (Input.GetButton("Horizontal") && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    direction = 1;
                    nextDashTime = Time.time + dashCoolDown;
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    direction = 2;
                    nextDashTime = Time.time + dashCoolDown;
                }
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                canMove = true;
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;
                canMove = false;
                if (direction == 1)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                }
                else if (direction == 2)
                {
                    rb.velocity = Vector2.right * dashSpeed;

                }
            }
        }
    }
}
