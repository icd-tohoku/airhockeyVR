using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuckCollision : MonoBehaviour {

    private float hitForce = 0.45f;
    private Rigidbody puckBody;
    private Text score;
    private static int p1score = 0, p2score = 0;

	void Start () {
        puckBody = gameObject.GetComponent<Rigidbody>();
        resetPos();

        if (Display.displays.Length > 1)
            Display.displays[1].Activate();

        score = GameObject.Find("Score").GetComponent<Text>();
        score.text = p1score.ToString() + "                                    " + p2score.ToString();
	}

    void FixedUpdate() {
        score.text = p1score.ToString() + "                                    " + p2score.ToString();

        Vector3 pos = puckBody.position;
        if(pos.z > -1.3 && pos.z < 0) {
            puckBody.AddForce(new Vector3(0, 0, -0.02f));
        } else if(pos.z < 1.3 && pos.z > 0) {
            puckBody.AddForce(new Vector3(0, 0, 0.02f));
        }

        if(pos.z > 2.3) {
            puckBody.AddForce(new Vector3(0, 0, -10f));
        } else if(pos.z < -2.3) {
            puckBody.AddForce(new Vector3(0, 0, 10f));
        }
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.name == "AirHockeyPad" || c.gameObject.name == "AirHockeyPad (1)") {
            Vector3 dir = transform.position - (c.contacts[0].point + new Vector3(0, transform.position.y, 0));
            puckBody.AddForce(dir * hitForce);
        } else if(c.gameObject.name == "AirHockeyTable") {
            Vector3 dir = transform.position - (new Vector3(c.contacts[0].point.x, transform.position.y, c.contacts[0].point.z));
            dir.z = -dir.z;
            dir.y = -dir.y;
            //puckBody.AddForce(puckBody.velocity.normalized * 5f);
            Debug.Log("ca tape la table");
        }
    }

    public void resetPos() {
        puckBody.position = new Vector3(0, 1.8f, 0);
        puckBody.AddForce(new Vector3(0, 0, -.5f));
    }

    public static void setP1() {
        p1score++;
    }

    public static void setP2() {
        p2score++;
    }
}
