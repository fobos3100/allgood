using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private Player player;
    public Transform firePoint;
    public Transform firePointUpper;
    public Transform firePointLower;
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
    //public GameObject impactEffect;
    public LineRenderer lineRenderer;

    private void Awake()
    {
        attackTrigger.enabled = false;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }


    void Update ()
    {
        if (Input.GetButton("Fire1"))
        {
            StartCoroutine(Ray());
            //Shoot();
            //Melee();
        }

	}

    void Shoot()
    {
        if (shootTime <= Time.time)
        {
            if (player.curMP > 0)
            {
                shootTime = Time.time + shootCD;
                if (Input.GetAxis("Vertical") > 0)
                {
                    Instantiate(bulletPrefab, firePointUpper.position, firePointUpper.rotation);
                    player.removeMP(1);
                }
                if (Input.GetAxis("Vertical") < 0 && !player.isGrounded)
                {
                    Instantiate(bulletPrefab, firePointLower.position, firePointLower.rotation);
                    player.removeMP(1);
                }
                else
                {
                    Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    player.removeMP(1);
                }
            }
       }        
    }

    void Melee()
    {
        attacking = true;
        attackTimer = attackCd;

        attackTrigger.enabled = true;

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

    IEnumerator Ray()       //not complete. LineRender
    {
        if (shootTime <= Time.time)
        {
            if (player.curMP > 0)
            {
                shootTime = Time.time + shootCD;
                RaycastHit2D hitInfo;

                if (Input.GetAxis("Vertical") > 0)
                {
                    hitInfo = Physics2D.Raycast(firePointUpper.position, firePoint.up);
                    player.removeMP(1);
                }
                else if (Input.GetAxis("Vertical") < 0 && !player.isGrounded)
                {
                    hitInfo = Physics2D.Raycast(firePointLower.position, -firePoint.up);
                    player.removeMP(1);
                }
                else
                {
                    hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
                    player.curMP -= 1;
                }

                if (hitInfo)
                {
                    Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(rayDamage);
                    }

                    //Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
                    if (Input.GetAxis("Vertical") > 0)
                    {
                        lineRenderer.SetPosition(0, firePointUpper.position);
                    }
                    else if (Input.GetAxis("Vertical") < 0 && !player.isGrounded)
                    {
                        lineRenderer.SetPosition(0, firePointLower.position);
                    }
                    else
                    {
                        lineRenderer.SetPosition(0, firePoint.position);
                    }

                    lineRenderer.SetPosition(1, hitInfo.point);
                }
                else
                {
                    if (Input.GetAxis("Vertical") > 0)
                    {
                        lineRenderer.SetPosition(0, firePointUpper.position);
                    }
                    else if (Input.GetAxis("Vertical") < 0 && !player.isGrounded)
                    {
                        lineRenderer.SetPosition(0, firePointLower.position);
                    }
                    else
                    {
                        lineRenderer.SetPosition(0, firePoint.position);
                    }
                    lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 1000);
                }

                lineRenderer.enabled = true;
                //wait one frame
                yield return 0;

                lineRenderer.enabled = false;
            }
        }
    }
}
