using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HERO_script : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //MOVE
    public float playerSpeed = 10f;
    public bool canMove;
    //JUMP
    private Vector2 jumpForce = new Vector2(0, 10f);
    public bool isGrounded;
    public bool canDoubleJump;
    //DASH
    private float dashSpeed=30f;
    private float startDashTime=0.1f;
    private int direction;
    private float dashTime;
    //COOLDOWN
    private float dashCoolDown=1f;
    private float nextDashTime = 0f;

    void Start()
    {
        canMove = true;
        isGrounded = true;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        dashTime = startDashTime;
    }

    private void Update()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");

        //float moveVertical = Input.GetAxis("Vertical");

        Move();
        Jump();
        Dash();
    }

    private void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D cl)
    {
        if (cl.gameObject.tag == "ground" && isGrounded == false)
        {
            isGrounded = true;
        }
    }

    void OnCollisionStay2D(Collision2D cl)
    {
        if (cl.gameObject.tag == "ground" && isGrounded == false)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D cl)
    {
        isGrounded = false;
        canDoubleJump = true;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().AddForce(jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            } else {
                if (canDoubleJump) {
                    canDoubleJump = false;
                    rb.velocity = Vector2.zero;
                    GetComponent<Rigidbody2D>().AddForce(jumpForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    void Move()
    {
        if (canMove == true)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.left * playerSpeed * Time.deltaTime, Space.World);
                sr.flipX = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * playerSpeed * Time.deltaTime, Space.World);
                sr.flipX = false;
            }
        }
    }

    void Dash()
    {
                if (direction == 0)
                {
                    if (Time.time > nextDashTime) {
                if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift))
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