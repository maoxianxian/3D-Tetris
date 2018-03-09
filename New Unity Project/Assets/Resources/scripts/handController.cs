using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handController{
	Leap.Hand lefthand;
	Leap.Hand righthand;
	Leap.Controller controller;
	GameObject grabbed;
	float ojbtime = 0;
	public handController(){
		controller= new Leap.Controller ();
	}

	public void connectToHands(){
		if (controller.Frame ().Hands.Count == 1) {
			Leap.Hand temp = controller.Frame ().Hands [0];
			if (temp.IsLeft) {
				lefthand = temp;
				righthand = null;
			} else {
				righthand = temp;
				lefthand = null;
			}
		} else if (controller.Frame ().Hands.Count == 2) {
			Leap.Hand temp = controller.Frame ().Hands [0];
			if (temp.IsLeft) {
				lefthand = temp;
				righthand = controller.Frame ().Hands [1];
			} else {
				righthand = temp;
				lefthand = controller.Frame ().Hands [1];
			}

		} else {
			lefthand = null;
			righthand = null;
		}
	}

	public void moveobj(){
	}

	void detectgrab(){
		if (grabbed == null) {
			
		}
	}

	Vector3 leapToUnity(Leap.Vector v)
	{
		Vector3 result = new Vector3(0,0,0);
		result.x = -v.x;
		result.y = -v.z;
		result.z = v.y;
		return result;
	}

	bool isFist(Leap.Hand h){
		int badfinger = 0;
		foreach (Leap.Finger fig in h.Fingers) {
			if (!fig.IsExtended) {
				badfinger++;
			}
		}
		if (badfinger > 3) {
			return true;
		}
		return false;
	}



}
