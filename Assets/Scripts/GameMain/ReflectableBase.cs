using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ReflectableBase : MonoBehaviour {
	/*v定数ゾーンv*/

	const float GRAVITY= 0.2f;        //重力
	const float LAND_FRICTION = 0.9f; //地面着地時の減衰率 高いほどビョンビョンする。
	/*^定数ゾーン^*/

	protected Rigidbody rigidbody;
	protected Vector3 velocity;//加工するvelocity
//	Collider collider;
	protected float height;//地面との距離バッファ
	float minPosY;//地面との最低距離
	float objectDistance;//接触相手との距離バッファ
	protected void AimValue(ref float value,float aim,float speed = 10){
		value += (aim - value) / speed; 
	}
	///<summary>[初期化]コライダーはトリガーに</summary>
	void Awake(){
		height = 0;
		rigidbody = GetComponent<Rigidbody> ();
//		collider = GetComponent<Collider> ();
//		collider.isTrigger = true;
	}
	void Start () {
		velocity = Vector3.zero;
		StartLate ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.AddForce (new Vector3(0,height*90,0));
		velocity = rigidbody.velocity;
		//velocity.y += height;
		//if(minPosY<-1f && velocity.y < 0)velocity.y=-velocity.y;
		if(height!=0)velocity.y *= LAND_FRICTION;
		velocity.y -= GRAVITY;
		//rigidbodyの更新前
		FixedUpdateLate ();

		rigidbody.velocity = velocity;
		height = 0;
		minPosY = 100;
	}

	#region Collision
	float D(Vector3 p,Vector3 n){return p.x * n.x + p.y * n.y + p.z * n.z;}
	float Distance(Vector3 landPos,Vector3 playerPos,Vector3 normal){
		return normal.x * playerPos.x+normal.y * playerPos.y+normal.z * playerPos.z-D(landPos,normal) / normal.magnitude;
	}
	///<summary>[地面接触判定]</summary>
	///<param name="collider">接触相手のCollider</param>
	void OnHitLand(Collider collider){
		ReflectableBase reflect;
		try {
			//Debug.Log(collider.transform.eulerAngles.x+90%180);
			//if((collider.transform.eulerAngles.x+90)%180!=0 &&(collider.transform.eulerAngles.z+90)%180!=0){
			//minPosY = collider.transform.position.y - (transform.position.z - collider.transform.position.z) * Mathf.Tan(collider.transform.eulerAngles.x * Mathf.PI/180)
			//										+ (transform.position.x - collider.transform.position.x) * Mathf.Tan(collider.transform.eulerAngles.z * Mathf.PI/180);
			//	height = -(transform.position.y - collider.transform.position.y)+0.5f - (transform.position.z - collider.transform.position.z) * Mathf.Tan(collider.transform.eulerAngles.x * Mathf.PI/180)
			//		+ (transform.position.x - collider.transform.position.x) * Mathf.Tan(collider.transform.eulerAngles.z * Mathf.PI/180);

			//}
			//Debug.Log(collider.transform.eulerAngles.z );
			//Debug.Log(collider.transform.eulerAngles.z );
			height = -0.1f-Distance(collider.transform.position,transform.position,collider.transform.up);
			//minPosY = Distance(collider.transform.position,transform.position,collider.transform.up);
			Debug.Log(Distance(collider.transform.position,transform.position,collider.transform.up));
			OnHitLandLate (collider);



		}       
		catch (NullReferenceException ex) {
			Debug.Log("RefrectableBaseまたはその派生クラスがアタッチされていない");
		}
	}
	///<summary>[プレイヤー接触判定]</summary>
	///<param name="collider">接触相手のCollider</param>
	void OnHitPlayer(Collider collider){
		PlayerBase reflect;
		try {
			reflect = collider.transform.parent.parent.GetComponent<PlayerBase> ();
			Vector3 distance = (reflect.transform.position - transform.position);
			rigidbody.AddForce (-distance.normalized *( 200 ));
			//Debug.Log(height);
			OnHitPlayerLate (reflect);
		}       
		catch (NullReferenceException ex) {
			Debug.Log("PlayerBaseまたはその派生クラスがアタッチされていない");
		}
	}
	void OnTriggerStay(Collider collider){
		if (collider.tag == "Land") {
			OnHitLand (collider);

		}
		if (collider.tag == "Player") {
			OnHitPlayer (collider);

		}
		
	}
	#endregion

	#region OverrideMethods
	virtual protected void StartLate (){}

	///<summary>[継承用:アップデート]rigidbody更新直前に呼び出し</summary>
	virtual protected void FixedUpdateLate (){}

	///<summary>[継承用:地面接触判定]地面接触時に呼び出し</summary>
	///<param name="reflect">接触相手のReflectableBase</param>
	virtual protected void OnHitLandLate(Collider reflect){}
	virtual protected void OnHitPlayerLate(PlayerBase reflect){}
	#endregion 
}
