using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VREnv : MonoBehaviour {
    private float baseFactor = 2.0f;
    private bool normalCam = false;

    [SerializeField] private Transform EnvRef = null;
    [SerializeField] private Camera VRCam = null;

    [SerializeField] private LayerMask VRNormal;
    [SerializeField] private LayerMask VRSpace;

    [SerializeField] private GameObject puck = null;
    [SerializeField] private GameObject DPad = null;
    [SerializeField] private GameObject VRPad = null;

    [SerializeField] private GameObject EmulatedPuck = null;
    [SerializeField] private GameObject EmulatedDPad = null;
    [SerializeField] private GameObject EmulatedVRPad = null;
    void Start () {

	}
	
	void Update () {
        EmulatedPuck.transform.position = new Vector3(puck.transform.position.x * baseFactor, 0, puck.transform.position.z * baseFactor) + EnvRef.position;
        EmulatedDPad.transform.position = new Vector3(DPad.transform.position.x * baseFactor, 0, DPad.transform.position.z * baseFactor) + EnvRef.position;
        EmulatedVRPad.transform.position = new Vector3(VRPad.transform.position.x * baseFactor, 0, VRPad.transform.position.z * baseFactor) + EnvRef.position;

        if (Input.GetKeyDown(KeyCode.D)) {
            if (normalCam) VRCam.cullingMask = VRNormal;
            else VRCam.cullingMask = VRSpace;
            normalCam = !normalCam;
        }
    }
}
