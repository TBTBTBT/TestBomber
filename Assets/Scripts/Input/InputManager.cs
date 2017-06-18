using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : InputManagerBase {


	[Header("入力の名前 '_L' および'_R' は省略すること")]
	public string inputHorizontalName;//この名前に_Lもしくは_Rがつく
	public string inputVerticalName; 
	// Use this for initialization

	// Update is called once per frame
	override protected void UpdateLate (){
		AxisToLever (ref leverLeft,Input.GetAxisRaw (inputHorizontalName + "_L"), Input.GetAxisRaw (inputVerticalName + "_L"));
		AxisToLever (ref leverRight,Input.GetAxisRaw (inputHorizontalName + "_R"), Input.GetAxisRaw (inputVerticalName  + "_R"));
	}

}
