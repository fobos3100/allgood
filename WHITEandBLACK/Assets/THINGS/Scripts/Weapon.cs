using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public Transform firePoint;
    public GameObject bulletPrefab;
    //missle
    private float shootTime = 0;
    public float shootCD = 0.5f;
    //melee
    private bool attacking = false;
    private float attackTimer = 0;
    public float attackCd = 0.3f;
    public Collider2D attackTrigger;
    //ray
    public int rayDamage = 3;

    private void Awake()
    {
        attackTrigger.enabled = false;
    }

    void Update ()
    {
        //Shoot();
        //Melee();
        Ray();
	}

    void Shoot()
    {
        if (Input.GetButton("Fire1"))
        {
            if (shootTime <= Time.time)
            {
                shootTime = Time.time + shootCD;
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }
        }
    }

    void Melee()
    {
        if (Input.GetButtonDown("Fire1") && !attacking)
        {
            attacking = true;
            attackTimer = attackCd;

            attackTrigger.enabled = true;
        }

        if (attacking)
        {
            if (attackTimer >= 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attacking = false;
                attackTrigger.enabled = false;
            }
        }
    }

    void Ray()
    {
        if (Input.GetButton("Fire1"))
        {
            if (shootTime <= Time.time)
            {
                shootTime = Time.time + shootCD;
                RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
                if (hitInfo)
                {
                    Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(rayDamage);
                    }
                }
            }
        }
    }
}
