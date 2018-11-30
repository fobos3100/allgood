using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTrigger : MonoBehaviour {

    private Player player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player")){
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.climbSpeed*Input.GetAxis("Horizontal"), player.climbSpeed*Input.GetAxis("Vertical"));
        }
    }
}
