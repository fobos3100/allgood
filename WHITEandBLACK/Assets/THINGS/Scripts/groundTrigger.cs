using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundTrigger : MonoBehaviour
{

    private Player hero;

    private void Start()
    {
        hero = gameObject.GetComponentInParent<Player>();
    }

    void OnTriggerEnter2D(Collider2D cl)
    {      
        hero.isGrounded = true;
    }

    void OnTriggerStay2D(Collider2D cl)
    {     
        hero.isGrounded = true;
    }

    void OnTriggerExit2D(Collider2D cl)
    {
        hero.isGrounded = false;
        hero.canDoubleJump = true;
    }
}      