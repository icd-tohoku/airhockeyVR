using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPad : MonoBehaviour {

	void Start () {
	}
	
	void FixedUpdate () {
        Vector3 v = GameObject.Find("AirHockeyPuck").GetComponent<Rigidbody>().velocity;
        Vector3 p = new Vector3(1, 0, 0);
        Vector3 a = GameObject.Find("AirHockeyPuck").GetComponent<Rigidbody>().position;
        Vector3 b = gameObject.GetComponent<Rigidbody>().position;

        Vector3 pos = new Vector3((a.x) * Time.deltaTime * 30f, 1.8415f, (a.z) * Time.deltaTime * 10f);
        if(pos.x < -0.75)  pos.x = -0.75f;
        if(pos.x > 0.75)  pos.x = 0.75f;
        if(pos.z < 0.5)  pos.z = 0.5f;
        
        gameObject.GetComponent<Rigidbody>().MovePosition(pos);
	}

    void OnCollisionEnter(Collision c) {
        
    }

    public static Vector3 LineLineIntersection(Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){
 
		Vector3 lineVec3 = linePoint2 - linePoint1;
		Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
		Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);
 
		float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);
 
		if(Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
		{
			float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
			return  linePoint1 + (lineVec1 * s);
		}
		else
		{
			return Vector3.zero;
		}
	}
}
