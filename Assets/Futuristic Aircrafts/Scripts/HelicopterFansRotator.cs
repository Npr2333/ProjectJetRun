using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterFansRotator : MonoBehaviour {

	public GameObject sideFanL;
	public GameObject sideFanR;
	public GameObject backFan;
	public float rotationSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		sideFanL.transform.Rotate (0, 10 * rotationSpeed * Time.deltaTime, 0);
		sideFanR.transform.Rotate (0, 10 * rotationSpeed * Time.deltaTime, 0);
		backFan.transform.Rotate (-10 * rotationSpeed * Time.deltaTime, 0 , 0);
	}
}
