using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPad : MonoBehaviour {
    private bool borderCollision;

	void Start () {
		borderCollision = false;
	}
	
	void FixedUpdate () {
        //Vector3 pos = new Vector3(-TrueMotionObjectVer.getPos().x * 3.5f, 1.8415f, -TrueMotionObjectVer.getPos().z * 4f - 0.5f);
        //if(pos.x < -0.75)  pos.x = -0.75f;
        //if(pos.x > 0.75)  pos.x = 0.75f;
        //if(pos.z < -2.3)  pos.z = -2.3f;

        //if(!borderCollision) {
        //    gameObject.GetComponent<Rigidbody>().MovePosition(pos);
        //}

	}

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.name == "AirHockeyTable") {
            borderCollision = true;
            //gameObject.GetComponent<Rigidbody>().position += new Vector3(c.contacts[0].normal.normalized.x * 0.01f, 1.8415f, c.contacts[0].normal.normalized.z * 0.01f);
        }
    }

    void OnCollisionExit(Collision c) {
        borderCollision = false;
    }
}
