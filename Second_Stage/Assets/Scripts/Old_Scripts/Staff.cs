using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Staff : MonoBehaviour {

    Player player;
    
    public Transform firePoint;
    public Transform weaponPoint;
    public Transform spearMovePoint;

    public GameObject bulletPrefab;
    public GameObject meleePrefab;

    //missle
    private float shootTime = 0;
    public float shootCD = 0.5f;
    //melee
    private float attackTime = 0;
    public float attackCd = 1.3f;
    //ray
    public int rayDamage = 3;
    //public GameObject impactEffect;
    public LineRenderer lineRenderer;

    //spear
    public float spearMoveSpd=100;
    public float spearTime=0;
    public float spearStopTime = 0;
    public float spearMoveTime = 3;
    public bool spearForward=true;
    //weapon selection
    public int weaponNumber = 0;
    public Text weaponText = null;
    public string [] weapons;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        weaponText.text = weapons[weaponNumber];
    }
    
    private void Update()
    {
        WeaponSelection();
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire2"))
        Shoot();
    }

    void Missle()
    {
        if (shootTime <= Time.time)
        {
            //if (player.curMP > 0)
            {
                shootTime = Time.time + shootCD;               
                    Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
           //         player.removeMP(1);
            }
        }
    }

    void Melee()
    {
        if (attackTime <= Time.time)
        {
            attackTime = Time.time + attackCd;
            Instantiate(meleePrefab, firePoint.position, firePoint.rotation);
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
                hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
           //     player.removeMP(1);

                if (hitInfo)        //ray visualisation
                {
                    Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(rayDamage);
                    }
                    lineRenderer.SetPosition(0, firePoint.position);
                    lineRenderer.SetPosition(1, hitInfo.point);
                }
                else
                {
                        lineRenderer.SetPosition(0, firePoint.position);
                        lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 1000);
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
        if (weapons[weaponNumber] == "Melee")
            Melee();
        else if (weapons[weaponNumber] == "Missle")
            Missle();
        else if (weapons[weaponNumber] == "Ray")
            StartCoroutine(Ray());
        else if (weapons[weaponNumber] == "Spear")
            StartCoroutine(Spear());
    }

    void WeaponSelection()
    {
        if (Input.GetButtonDown("Weapon0"))
        {
            weaponNumber = 0;
            weaponText.text = weapons[weaponNumber];
        }
        else if (Input.GetButtonDown("Weapon1"))
        {
            weaponNumber = 1;
            weaponText.text = weapons[weaponNumber];
        }
        else if (Input.GetButtonDown("Weapon2"))
        {
            weaponNumber = 2;
            weaponText.text = weapons[weaponNumber];
        }
        else if (Input.GetButtonDown("Weapon3"))
        {
            weaponNumber = 3;
            weaponText.text = weapons[weaponNumber];
        }
        else if (Input.GetButtonDown("Weapon4"))
        {
            weaponNumber = 4;
            weaponText.text = weapons[weaponNumber];
        }
    }

    IEnumerator Spear()
    {
        if (spearForward)
        {
            Debug.Log("AAAA");
            //transform.position = Vector2.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * spearMoveSpd);
            transform.position = Vector2.MoveTowards(transform.position, spearMovePoint.transform.position , Time.deltaTime*spearMoveSpd);
            yield return null;
            if (transform.position == spearMovePoint.transform.position)
            {
                spearForward = !spearForward;
            }
        }

        if (!spearForward)
        {
            transform.position = Vector2.MoveTowards(transform.position, weaponPoint.transform.position, Time.deltaTime * spearMoveSpd);
            yield return null;
            if (transform.position == weaponPoint.transform.position)
            {
                spearForward = !spearForward;
            }
        }
        if (Input.GetButtonUp("Fire2"))
        {
            transform.position = weaponPoint.transform.position;
        }
    }
}
