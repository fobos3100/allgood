using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HERO_script : MonoBehaviour {

    private Rigidbody2D playerRB;
    private SpriteRenderer playerSR;

    public float playerSpeed;
    public Vector2 jumpForce;

    public bool isGrounded;

	// Use this for initialization
	void Start () {
        isGrounded = true;
        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        //float moveHorizontal = Input.GetAxis("Horizontal");

        //float moveVertical = Input.GetAxis("Vertical");

        move();
        jump();
	}

    private void FixedUpdate()
    {
        
    }

    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&isGrounded)
        {
            GetComponent<Rigidbody2D>().AddForce(jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
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

    void OnCollisionEnter2D (Collision2D playerCL)
    {
        if (playerCL.gameObject.tag == "ground" && isGrounded == false)
        {
            isGrounded = true;
        }
    }

}
