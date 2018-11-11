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

    void OnCollisionEnter2D(Collision2D cl)
    {      
            hero.isGrounded = true;
    }

    void OnCollisionStay2D(Collision2D cl)
    {     
            hero.isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D cl)
    {
        hero.isGrounded = false;
        hero.canDoubleJump = true;
    }
}      