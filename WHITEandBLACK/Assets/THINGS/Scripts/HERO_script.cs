using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HERO_script : MonoBehaviour
{

    private Rigidbody2D playerRB;
    private SpriteRenderer playerSR;
    //MOVE
    public float playerSpeed = 10f;
    public bool CanMove;
    //JUMP
    public Vector2 JumpForce = new Vector2(0, 10f);
    public bool isGrounded;
    //DASH
    public float DashSpeed;
    public float StartDashTime;
    private int Direction;
    private float DashTime;


    // Use this for initialization
    void Start()
    {
        CanMove = true;
        isGrounded = true;
        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
        DashTime = StartDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");

        //float moveVertical = Input.GetAxis("Vertical");

        move();
        jump();
        dash();
    }

    private void FixedUpdate()
    {

    }

    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            GetComponent<Rigidbody2D>().AddForce(JumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D playerCL)
    {
        if (playerCL.gameObject.tag == "ground" && isGrounded == false)
        {
            isGrounded = true;
        }
    }

    void move()
    {
        if (CanMove == true)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.left * playerSpeed * Time.deltaTime, Space.World);
                playerSR.flipX = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * playerSpeed * Time.deltaTime, Space.World);
                playerSR.flipX = false;
            }
        }
    }

    void dash()
    {

        {

            if (Direction == 0)
            {
                if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Direction = 1;
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Direction = 2;
                }
            }
            else
            {
                if (DashTime <= 0)
                {
                    CanMove = true;
                    Direction = 0;
                    DashTime = StartDashTime;
                    playerRB.velocity = Vector2.zero;
                }
                else
                {
                    DashTime -= Time.deltaTime;
                    CanMove = false;
                    if (Direction == 1)
                    {
                        playerRB.velocity = Vector2.left * DashSpeed;
                    }
                    else if (Direction == 2)
                    {
                        playerRB.velocity = Vector2.right * DashSpeed;
                    }
                }
            }        
        }
    }
}