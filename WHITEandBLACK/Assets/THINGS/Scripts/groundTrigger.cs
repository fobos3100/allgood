using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundTrigger : MonoBehaviour
{

    private HERO_script hero;

    private void Start()
    {
        hero = gameObject.GetComponentInParent<HERO_script>();
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