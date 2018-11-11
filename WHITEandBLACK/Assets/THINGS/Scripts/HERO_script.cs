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
    private float moveSpeed = 100f;
    private float maxSpeed = 150f;
    public static bool canMove;
    //JUMP
    private Vector2 jumpForce = new Vector2(0, 150f);
    public bool isGrounded;
    public bool canDoubleJump;
    public float fallMultiplier = 2.5f;     
    public float lowJumpMultiplier = 2f;

    public Collider2D groundTrigger;

    //DASH
    private float dashSpeed = 200f;   
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

    void Awake()
    {
        attackTrigger.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        canMove = true;
        isGrounded = true;
        dashTime = startDashTime;
        currentHP = maxHP;
    }

    private void Update()
    {
        Jump();
        Dash();
        Attack();
        Health(currentHP);
    }

    private void FixedUpdate()
    {
        Move();
    }

    void OnCollisionEnter2D(Collision2D cl)
    {

    }

    void OnCollisionStay2D(Collision2D cl)
    {   
        if (cl.gameObject.tag == "Damage" && Time.time > nextDamageTime)
        {
            currentHP -= 1;
            nextDamageTime = Time.time + takeDamageCoolDown;
        }
    }

    private void OnCollisionExit2D(Collision2D cl)
    {

    }


    private void Jump()
    {
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
                    rb.velocity = Vector2.zero;
                    GetComponent<Rigidbody2D>().AddForce(jumpForce, ForceMode2D.Impulse);
                    isGrounded = false;
                    Debug.Log("Jumped");
                }
                else if (canDoubleJump)
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