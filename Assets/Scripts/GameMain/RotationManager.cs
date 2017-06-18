using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour {
	float angleX = 30;
	float angleZ = 30;
	Vector3 aim;
	Vector3 now;
	public GameObject Player;
	// Use this for initialization
	void Start () {
		aim = new Vector3(0,0,0);
		now = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.localPosition = Player.transform.position;
		now = aim;
		transform.localRotation = Quaternion.Euler (now);
		aim = new Vector3(0,0,0);

	}
	public void SetAimAngle(Vector3 aim){
		this.aim = aim;
	}
}
