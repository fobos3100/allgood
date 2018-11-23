using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HERO_script : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //HP
    public int curHP;
    public int maxHP = 10;
    private float takeDamageCD = 1f;
    private float nextDamageTime = 0f;
    //MP
    public int curMP;
    public int maxMP=16;
    private float MPregenCD=1f;
    private float nextMPregenTime=1f;
    //MOVE
    private float moveSpeed = 200f;
    private float maxSpeed = 210f;
    public static bool canMove;
    //JUMP
    private float jumpForce = 7000f;
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
    private float dashCD = 1f;
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
        curHP = maxHP;
        curMP = maxMP;
        StartCoroutine(addMana());
    }

    private void Update()
    {
        Jump();
        Dash();
        Attack();
        Health(curHP);
        Mana(curMP);
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
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1)  * Time.deltaTime ;
                }
                else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime ;
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
                    nextDashTime = Time.time + dashCD;
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    direction = 2;
                    nextDashTime = Time.time + dashCD;
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

    IEnumerator addMana()
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

    void Attack()
    {
        if(Input.GetButtonDown("Fire1")&&!attacking)
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

    public void Damage(int dmg)
    {
        curHP -= dmg;
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