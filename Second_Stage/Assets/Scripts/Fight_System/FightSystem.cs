using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSystem : MonoBehaviour {

    Player player;

    public GameObject bulletPrefab;

    public Transform firePoint;

    //missle
    private float shootTime = 0;
    public float shootCD = 0.5f;

    void Start() {
        player = GetComponent<Player>();
    }

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Missle();
        }
    }

    void Missle() {
        if (shootTime <= Time.time) {
            //if (player.curMP > 0)
            {
                shootTime = Time.time + shootCD;
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                //         player.removeMP(1);
            }
        }
    }
}
