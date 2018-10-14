using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HERO_script : MonoBehaviour {

    private Rigidbody2D playerRB;
    private SpriteRenderer playerSR;

    public float playerSpeed=10f;
    public Vector2 JumpForce= new Vector2 (0,10f);
    public bool isGrounded;

    public float DashSpeed;
    public float StartDashTime;
    private int direction;
    private float DashTime;
    

    // Use this for initialization
    void Start () {
        isGrounded = true;
        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
        DashTime = StartDashTime;
	}
	
	// Update is called once per frame
	void Update () {
        //float moveHorizontal = Input.GetAxis("Horizontal");

        //float moveVertical = Input.GetAxis("Vertical");
        DashTime = StartDashTime;

        move();
        jump();
        dash();
	}

    private void FixedUpdate()
    {
        
    }

    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&isGrounded)
        {
            GetComponent<Rigidbody2D>().AddForce(JumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D (Collision2D playerCL)
    {
        if (playerCL.gameObject.tag == "ground" && isGrounded == false)
        {
            isGrounded = true;
        }
    }

    void move()
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

    void dash()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashTime -= Time.deltaTime;
            playerRB.velocity = Vector2.left * DashSpeed;
        }
        if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashTime -= Time.deltaTime;
            playerRB.velocity = Vector2.right * DashSpeed;
        }
        
    }
}
