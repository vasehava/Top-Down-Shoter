using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraHealthBar : MonoBehaviour {
    private Camera cam = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (cam) transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform.rotation * Vector3.up);
        else cam = Camera.main;
    }
}
