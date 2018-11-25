using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    private Player Hero;

    //public int dmg=1;

    void Start()
    {

        Hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Hero.Damage(1);
            StartCoroutine(Hero.Knockback(0.02f, 10, Hero.transform.position));
        }

    }
}