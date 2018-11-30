using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundTrigger : MonoBehaviour
{

    private Player player;

    private void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
            player.isGrounded = true;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
            player.isGrounded = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            player.isGrounded = false;
            player.canDoubleJump = true;
        }
    }
}      