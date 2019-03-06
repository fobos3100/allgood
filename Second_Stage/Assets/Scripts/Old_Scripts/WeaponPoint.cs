using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPoint : MonoBehaviour {

    Staff staff;
    public Transform staffRestPoint;
    public float offset;

	// Use this for initialization
	void Start () {
        staff = GameObject.FindGameObjectWithTag("Staff").GetComponent<Staff>();
	}
	
	// Update is called once per frame
	void Update () {
        if (staff.weaponNumber != 0)
        {
            Rotating();
        }
	}

    void Rotating()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
    }
}
