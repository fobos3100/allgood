using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {

    Player player;
    public GameObject anotherDoorPoint;
    public Text lowerText=null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Use"))
            {
                player.transform.position = anotherDoorPoint.transform.position;
            }

            lowerText.text = "Press E to enter";
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        lowerText.text = null;
    }
}
