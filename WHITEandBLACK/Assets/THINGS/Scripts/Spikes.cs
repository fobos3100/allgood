using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    private Player player;

    //public int dmg=1;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.Damage(1);
            StartCoroutine(player.Knockback(0.02f, 10, player.transform.position));
        }

    }
}