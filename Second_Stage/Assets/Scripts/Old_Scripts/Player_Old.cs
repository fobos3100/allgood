using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Old : MonoBehaviour
{
    //other
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;

    private bool facingRight;

    //HP
    public int curHP=0;
    public int maxHP = 10;
    private float takeDmgCD = 1f;
    private float nextDmgTime = 0f;
    //MP
    public int curMP=0;
    public int maxMP = 16;
    private float MPregenCD = 1f;
    private float nextMPregenTime = 1f;
    //MOVE
    public float moveSpeed = 50f;
    public float maxSpeed = 210f;
    public static bool canMove;
    //JUMP
    public float jumpForce = 7000f;
    public bool isGrounded;
    public bool canDoubleJump;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public Collider2D groundTrigger;
    //DASH
    public float dashSpeed = 700f;
    private float dashStopTime=0;
    public float dashAirTime = 0.1f;
    private float dashTime=0;
    public float dashCD = 3f;
    private float dashDirection=0;
    //Climb
    public float climbSpeed=200;
    //Test
    //public CharacterController2D controller;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        facingRight = true;
        canMove = true;
        isGrounded = true;
        curHP = maxHP;
        curMP = maxMP;
        StartCoroutine(addMP());
    }

    private void Update()
    {
        Jump();
        Health(curHP);
        Mana(curMP);
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Move(h,v);
        Dash();
        Flip(h);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
    }

    void OnCollisionStay2D(Collision2D col)
    {       
    }

    private void OnCollisionExit2D(Collision2D col)
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
                    rb.AddForce(Vector2.up * jumpForce);
                    isGrounded = false;
                }
                else if (canDoubleJump)
                {
                    canDoubleJump = false;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector2.up * jumpForce);
                }
            }
        }
    }

    private void Flip(float h)
    {
        if (h>0 && !facingRight || h < 0 && facingRight)
        {
            facingRight = !facingRight;

            transform.Rotate(0f, 180f, 0f);
        }
    }

    void Move(float h, float v)
    {
        if (canMove == true)
        {
            rb.AddForce(Vector2.right * moveSpeed * h);

            if (rb.velocity.x > maxSpeed)
            {
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            }

            if (rb.velocity.x < -maxSpeed)
            {
                rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
            }
        }
    }

    void Dash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            if (dashTime <= Time.time)
            {
                dashTime = Time.time + dashCD;
                dashStopTime = Time.time + dashAirTime;
                dashDirection = Input.GetAxis("Horizontal");
            }
        }

        if (dashStopTime >= Time.time)
        {
            rb.velocity = new Vector2(dashSpeed * dashDirection, 0);
        }
    }

    void Health(int curHP)
    {
        if (curHP > maxHP)
        {
            curHP = maxHP;
        }

        if (curHP <= 0)
        {
            Debug.Log("Hero dies");
            Application.LoadLevel(Application.loadedLevel); //restart
        }
    }

    void Mana(int curMP)
    {
        if (curMP > maxMP)
        {
            curMP = maxMP;
        }
    }

    IEnumerator addMP()
    {
        while (true)
        { // loops forever...
            if (curMP < maxMP)
            { // if health < 100...
                curMP += 1; // increase MP and wait the specified time
                yield return new WaitForSeconds(1);
            }
            else
            { // if curMP >= maxMP, just yield 
                yield return null;
            }
        }
    }

    public void removeMP(int MP)
    {
        curMP -= MP;
    }

    public void Damage(int dmg)
    {
        if (nextDmgTime <= Time.time)
        {
            nextDmgTime = Time.time + takeDmgCD;
            curHP -= dmg;
        }
    }

    public IEnumerator Knockback(float knockDur, float knockbackPwr, Vector3 knockbackDir)
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