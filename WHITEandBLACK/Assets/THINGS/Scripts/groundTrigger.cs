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
        if (cl.gameObject.tag == "ground" && hero.isGrounded == false)
        {
            hero.isGrounded = true;
            Debug.Log("Collision Enter");
        }
    }

    void OnCollisionStay2D(Collision2D cl)
    {
        if (cl.gameObject.tag == "ground" && hero.isGrounded == false)
        {
            hero.isGrounded = true;
            Debug.Log("Collision Stay");
        }
    }

    void OnCollisionExit2D(Collision2D cl)
    {
        hero.isGrounded = false;
        hero.canDoubleJump = true;
        Debug.Log("Collision Exit");
    }
}      