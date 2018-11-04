using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;       //Public variable to store a reference to the player game object
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    private float downButtonTime;
    private float cameraSpeed=10f;

    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    private void Update()
    {
       
    }
    // LateUpdate is called after Update each frame
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W))
        {
            downButtonTime = Time.time;
        }
        if (Input.GetKey(KeyCode.S) && Time.time - downButtonTime >= 1)
        {
            MoveCameraDown();
        }
        else if(Input.GetKey(KeyCode.W) && Time.time - downButtonTime >= 1)
        {
            MoveCameraUp();
        }
        else
        {
            HERO_script.canMove = true;
            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = player.transform.position + offset;
        }
    }    

    void MoveCameraDown()
    {
        HERO_script.canMove = false;
        if (Time.time - downButtonTime <= 3)
        {
            transform.Translate(new Vector3(0, -cameraSpeed * Time.deltaTime, 0));
        }
    }

    void MoveCameraUp()
    {
        HERO_script.canMove = false;
        if (Time.time - downButtonTime <= 2)
        {
            transform.Translate(new Vector3(0, cameraSpeed * Time.deltaTime, 0));
        }
    }
}