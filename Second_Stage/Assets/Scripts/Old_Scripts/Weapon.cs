using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {

    private Player player;
    public Transform firePoint;
    public Transform firePointUpper;
    public Transform firePointLower;
    public GameObject bulletPrefab;
    public GameObject meleePrefab;
    //missle
    private float shootTime = 0;
    public float shootCD = 0.5f;
    //melee
    private float attackTime = 0;
    public float attackCd = 1.3f;
    public Transform meleeFirePoint;
    //ray
    public int rayDamage = 3;
    //public GameObject impactEffect;
    public LineRenderer lineRenderer;
    //weapon selection
    public int weaponNumber=0;
    public Text weaponText = null;
    public string weapon0 = "none";
    public string weapon1 = "Melee";
    public string weapon2 = "Missle";
    public string weapon3 = "Ray";


    private void Awake()
    {
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        weaponText.text = "none";
        weaponNumber = 0;
    }

    private void FixedUpdate ()
    {
        WeaponSelection();

        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }

	}

    void Missle()
    {
        if (shootTime <= Time.time)
        {
           // if (player.curMP > 0)
            {
                shootTime = Time.time + shootCD;
                if (Input.GetAxis("Vertical") > 0)
                {
                    Instantiate(bulletPrefab, firePointUpper.position, firePointUpper.rotation);
               //     player.removeMP(1);
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    Instantiate(bulletPrefab, firePointLower.position, firePointLower.rotation);
             //       player.removeMP(1);
                }
                else if(Input.GetAxis("Vertical") == 0)
                {
                    Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
              //      player.removeMP(1);
                }
            }
        }        
    }

    void Melee()
    {
        if (attackTime <= Time.time)
        {
            attackTime = Time.time + attackCd;
            Instantiate(meleePrefab, meleeFirePoint.position, meleeFirePoint.rotation);
        }      
    }

    IEnumerator Ray()       
    {
        if (shootTime <= Time.time)
        {
           // if (player.curMP > 0)
            {
                shootTime = Time.time + shootCD;
                RaycastHit2D hitInfo;

                if (Input.GetAxis("Vertical") > 0)
                {
                    hitInfo = Physics2D.Raycast(firePointUpper.position, firePoint.up);
               //     player.removeMP(1);
                }
             //   else if (Input.GetAxis("Vertical") < 0 && !player.isGrounded)
                {
                    hitInfo = Physics2D.Raycast(firePointLower.position, -firePoint.up);
               //     player.removeMP(1);
                }
               // else
                {
                    hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
              //      player.removeMP(1);
                }

                if (hitInfo)        //ray visualisation
                {
                    Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(rayDamage);
                    }

                    //StartCoroutine(player.Knockback(0.02f, 3, new Vector2(1,0,0)));

                    //Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
                    if (Input.GetAxis("Vertical") > 0)
                    {
                        lineRenderer.SetPosition(0, firePointUpper.position);
                    }
                //    else if (Input.GetAxis("Vertical") < 0 && !player.isGrounded)
                    {
                        lineRenderer.SetPosition(0, firePointLower.position);
                    }
           //         else
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
                        lineRenderer.SetPosition(1, firePoint.position + firePoint.up * 1000);
                    }
             //       else if (Input.GetAxis("Vertical") < 0 && !player.isGrounded)
                    {
                        lineRenderer.SetPosition(0, firePointLower.position);
                        lineRenderer.SetPosition(1, firePoint.position - firePoint.up * 1000);
                    }
             //       else
                    {
                        lineRenderer.SetPosition(0, firePoint.position);
                        lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 1000);
                    }
                }

                lineRenderer.enabled = true;
                //wait one frame
                yield return 0;

                lineRenderer.enabled = false;
            }
        }
    }

    void Shoot()
    {
        if (weaponNumber == 1)      
        Melee();      
        else if (weaponNumber == 2)
        Missle();
        else if (weaponNumber == 3)
        StartCoroutine(Ray());
    }

    void WeaponSelection()
    {
        if (Input.GetButtonDown("Weapon0"))
        {
            weaponNumber = 0;
            weaponText.text = weapon0;
        }
        else if (Input.GetButtonDown("Weapon1"))
        {
            weaponNumber = 1;
            weaponText.text = weapon1;
        }
        else if (Input.GetButtonDown("Weapon2"))
        {
            weaponNumber = 2;
            weaponText.text = weapon2;
        }
        else if (Input.GetButtonDown("Weapon3"))
        {
            weaponNumber = 3;
            weaponText.text = weapon3;
        }
    }
}
