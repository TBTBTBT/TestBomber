using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : InputManagerBase{

	// Use this for initialization
	override protected void StartLate () {
		
	}
	
	// Update is called once per frame
	override protected void UpdateLate () {
		AxisToLever (ref leverRight,1, 1);
	}
}
