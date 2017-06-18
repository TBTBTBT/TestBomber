using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerBase : ReflectableBase{
	public InputManagerBase input;

	float angle = 0;
	float leftSpeed;
	float rightSpeed;
	Vector3 eular = new Vector3 (0, 0, 0);
	Vector3 aimEular = new Vector3 (0, 0, 0);
	public GameObject rotateObject;
	[System.NonSerialized]public float speedMagnitude;
	// Use this for initialization
	override protected void StartLate (){
		angle = 0;
		leftSpeed = 0;
		rightSpeed = 0;
	}
	protected void AngleLimit(ref float angle,float limit = 360){
		angle = angle % limit;
	}
	protected void AngleLimit(ref Vector3 angle,float limit = 360){
		AngleLimit (ref angle.x);
		AngleLimit (ref angle.y);
		AngleLimit (ref angle.z);
	}
	protected void AngleAim(ref float angle,float aim,float speed = 10){
		AngleLimit (ref angle);
		AngleLimit (ref aim);
		if (Mathf.Abs (angle - aim) > 180) {
			if (aim > angle) {
				angle += ((aim - 360) - angle) / speed; 
			} else {
				angle += ((aim + 360) - angle) / speed; 
			}
		} else {
			angle += (aim - angle) / speed; 
		}
		AngleLimit (ref angle);
	}
	// Update is called once per frame
	override protected void FixedUpdateLate () {
		if (input.leverLeft > 0) {
			leftSpeed++;
			//Debug.Log (angle);
		}
		if (input.leverRight> 0) {
			rightSpeed++;
		}
		speedMagnitude = leftSpeed + rightSpeed;
		angle+=(leftSpeed - rightSpeed)/2;
		rigidbody.AddForce(rotateObject.transform.forward * (leftSpeed + rightSpeed) * 2);
		AimValue (ref leftSpeed,0,6);
		AimValue (ref rightSpeed,0,6);
		AimValue (ref velocity.x ,0,20);
		AimValue (ref velocity.z ,0,20);
		AngleLimit (ref eular);
		AngleLimit (ref aimEular);
		AngleLimit (ref angle);
		AngleAim(ref eular.x ,aimEular.x,20);
		AngleAim(ref eular.y ,aimEular.y,20);
		AngleAim(ref eular.z ,aimEular.z,20);
		transform.rotation = Quaternion.Euler (eular);//Quaternion.AngleAxis (angle, up.normalized);
		//Debug.Log(transform.up);
		rotateObject.transform.localRotation = Quaternion.AngleAxis (angle, new Vector3(0,1,0));
		aimEular = Vector3.zero;
	}

	protected override void OnHitLandLate(Collider reflect){
		Vector3 e = reflect.transform.eulerAngles;
		aimEular = e;



	}
	protected override void OnHitPlayerLate(PlayerBase reflect){
		Vector3 distance = (reflect.transform.position - transform.position);

		rigidbody.AddForce (reflect.velocity.normalized *( (reflect.speedMagnitude + speedMagnitude/5)*20 ));
	}
}
