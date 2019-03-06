using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector2 velocity;

    public GameObject player;       //Public variable to store a reference to the player game object

    private float downButtonTime;
    private float cameraSpeed=50f;
    public float smoothTimeX;
    public float smoothTimeY;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
    }

    private void LateUpdate()
    {


        if (Input.GetButtonDown("Vertical"))
        {
            downButtonTime = Time.time;
        }
        if (Input.GetButton("Vertical") && Time.time - downButtonTime >= 1)
        {
            Player.canMove = false;
            if (Time.time - downButtonTime <= 2)
            {
                float v = Input.GetAxis("Vertical");
                transform.Translate(new Vector3(0, cameraSpeed * Time.deltaTime * v, 0));
            }
        }
        else
        {
            Player.canMove = true;
            float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
            float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);
            transform.position = new Vector3(posX, posY, transform.position.z);
        }
    }    
}