using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAtk : MonoBehaviour {

    //private Weapon weapon;
    private int meleeDmg=5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(Destroy());
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(meleeDmg);
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);       
    }

}
