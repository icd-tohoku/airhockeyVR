using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {
    private Rigidbody puckBody;

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider c) {
        if (c.gameObject.name == "AirHockeyPuck") {
            if(gameObject.name == "P1Goal") {
                PuckCollision.setP2();
            } else if (gameObject.name == "P2Goal") {
                PuckCollision.setP1();
            }

            GameObject.Find("AirHockeyPuck").GetComponent<Rigidbody>().position = new Vector3(0, 1.8f, 0);
            GameObject.Find("AirHockeyPuck").GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -.5f));
        }
    }
}
