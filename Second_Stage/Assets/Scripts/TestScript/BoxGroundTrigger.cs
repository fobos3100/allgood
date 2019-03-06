using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGroundTrigger : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D cl)
    {
        Debug.Log("CollisionEnter");
        Debug.Log(cl.gameObject.tag);
    }

    void OnCollisionStay2D(Collision2D cl)
    {
        Debug.Log("CollisionStay");
        Debug.Log(cl.gameObject.tag);
    }

    void OnCollisionExit2D(Collision2D cl)
    {
        Debug.Log("CollisionExit");
        Debug.Log(cl.gameObject.tag);
    }
}
