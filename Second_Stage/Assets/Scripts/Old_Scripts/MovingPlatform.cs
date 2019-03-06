using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public GameObject player;
    public GameObject[] waypoints;
    public int current = 0;
    public float rotSpeed=100;
    public float movespeed=100;
    public float WPradius = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            player.transform.parent = transform;
        }   
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            player.transform.parent = null;
        }
    }

    void Update () {
        if (Vector2.Distance(waypoints[current].transform.position, transform.position) < WPradius)
        {
            current++;
            if (current >= waypoints.Length)
            {
                current = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * rotSpeed);
	}
}
