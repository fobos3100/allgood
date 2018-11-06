using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HERO_script : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //HP
    private int currentHP;
    private int maxHP = 3;
    private float takeDamageCoolDown = 1f;
    private float nextDamageTime = 0f;
    //MOVE
    private float playerSpeed = 100f;        //change with size
    public static bool canMove;
    //JUMP
    private Vector2 jumpForce = new Vector2(0, 100f);       //change with size
    public bool isGrounded;
    public bool canDoubleJump;
    //DASH
    private float dashSpeed = 150f;     //change with size
    private float startDashTime = 0.1f;
    private int direction;
    private float dashTime;
    private float dashCoolDown = 1f;
    private float nextDashTime = 0f;
    //ATTACK
    private bool attacking = false;
    private float attackTimer = 0;
    private float attackCd = 0.3f;

    public Collider2D attackTrigger;

    void Avake()
    {
        attackTrigger.enabled = false;
    }

    void Start()
    {
        canMove = true;
        isGrounded = true;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        dashTime = startDashTime;
        currentHP = maxHP;
    }

    private void Update()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");

        //float moveVertical = Input.GetAxis("Vertical");

        Move();
        Jump();
        Dash();
        Attack();
        Health(currentHP);
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

        if (cl.gameObject.tag == "Damage" && Time.time > nextDamageTime)
        {
            currentHP -= 1;
            nextDamageTime = Time.time + takeDamageCoolDown;
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
            }
            else
            {
                if (canDoubleJump)
                {
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
            if (Time.time > nextDashTime)
            {
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

    void Health(int currentHP)
    {
        if (currentHP == 0)
        {
            Debug.Log("Hero dies");
        }
    }

    void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Z)&&!attacking)
        {
            attacking = true;
            attackTimer = attackCd;

            attackTrigger.enabled = true;
        }

        if (attacking)
        {
            if (attackTimer >= 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attacking = false;
                attackTrigger.enabled = false;
            }
        }
    }
}