using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookRotations : MonoBehaviour {

	public Transform[] RingsAndCube;

	private Vector3[] _ringAndCubeSpeeds;

	// Use this for initialization
	void Start () {
		_ringAndCubeSpeeds = new Vector3[4];

		_ringAndCubeSpeeds[0] = new Vector3(0, -Random.Range(2, 4), -Random.Range(2, 4));
		_ringAndCubeSpeeds[1] = new Vector3(0, Random.Range(2, 4), 0);
		_ringAndCubeSpeeds[2] = new Vector3(0, 0, Random.Range(2, 4));
		_ringAndCubeSpeeds[3] = new Vector3(0, -Random.Range(1, 3), Random.Range(1, 3));
	}
	
	// Update is called once per frame
	void Update () {
		
		for(int i = 0; i < RingsAndCube.Length; i++)
		{
			var currentRotation = RingsAndCube[i].localEulerAngles;
			currentRotation -= _ringAndCubeSpeeds[i];
			RingsAndCube[i].localEulerAngles = currentRotation;
		}
	}
}
