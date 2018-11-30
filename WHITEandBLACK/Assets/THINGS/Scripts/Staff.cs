using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour {

    Player player;

    public float offset;

    public Transform firePoint;

    public GameObject bulletPrefab;

    //missle
    private float shootTime = 0;
    public float shootCD = 0.5f;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire2"))
        Missle();
    }

    void Missle()
    {
        if (shootTime <= Time.time)
        {
            if (player.curMP > 0)
            {
                shootTime = Time.time + shootCD;               
                    Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    player.removeMP(1);
            }
        }
    }

}
