using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour
{

    public int meleeDmg = 20;

    /*void OnTriggerEnter2D(Collider2D col)
    {

        if (col.isTrigger != true && col.CompareTag("Enemy"))
        {
            col.SendMessageUpwards("Damage", dmg);
        }
    }
    */

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(meleeDmg);
        }
    }
}
