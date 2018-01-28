using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorControl : MonoBehaviour {

	public Vector3 RotationSpeed;

	// Update is called once per frame
	void Update () {

		var currentRotation = transform.localEulerAngles;
		currentRotation += RotationSpeed * Time.deltaTime;
		transform.localEulerAngles = currentRotation;
	}
}
