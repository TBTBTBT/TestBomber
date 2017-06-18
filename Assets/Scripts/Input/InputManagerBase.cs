using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerBase : MonoBehaviour {
	/*v定数ゾーンv*/

	const float AXIS_LIMIT= 0.3f;
	int[,] LEVER = new int[,]
	{
		{7,8,9},//上
		{4,0,6},
		{1,2,3} //下
	};
	/*^定数ゾーン^*/

	[System.NonSerialized]public int leverLeft;
	[System.NonSerialized]public int leverRight;
	[System.NonSerialized]public int buttonLeft;
	[System.NonSerialized]public int buttonRight;

	// Use this for initialization
	void Start () {
		leverLeft = 0;
		leverRight = 0;
		StartLate ();
	}
	bool OverflowCheck(int firstIndex,int secondIndex){
		if (LEVER.GetLength(0) > firstIndex && LEVER.GetLength(1) > secondIndex) {
			return true;
		}
		return false;

	}
	protected void AxisToLever(ref int lever,float axisX,float axisY){
		
		int firstIndex = 1;
		int secondIndex = 1;
		if (Mathf.Abs(axisX) > AXIS_LIMIT) {
			secondIndex = (int)(Mathf.Abs (axisX) / axisX) + 1;

		}
		if (Mathf.Abs(axisY) > AXIS_LIMIT) {
			firstIndex = 1 - (int)(Mathf.Abs (axisY) / axisY) ;
		}
		if (OverflowCheck (firstIndex, secondIndex)) {
			lever = LEVER[firstIndex,secondIndex];
		}

	}
	// Update is called once per frame
	void Update () {
		UpdateLate ();
		//Debug.Log (leverLeft);
	}
	#region OverrideMethods
	virtual protected void StartLate (){}

	///<summary>[継承用:アップデート]rigidbody更新直前に呼び出し</summary>
	virtual protected void UpdateLate (){}

	#endregion 
}
