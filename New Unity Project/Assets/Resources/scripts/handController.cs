using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handController{
	Leap.Hand lefthand;
	Leap.Hand righthand;
	Leap.Controller controller;
	GameObject grabbed;
	float ojbtime = 0;
	Vector3 leaporigin;
	GameObject leapspace;
	LineRenderer debug;

	public handController(){
		leapspace = GameObject.Find ("CenterEyeAnchor");
		controller= new Leap.Controller ();
		//debug = DrawLine (Vector3.zero, Vector3.zero, Color.blue,0.02f);

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
		leaporigin = leapspace.transform.position;
		if (righthand != null) {
			//GameObject.Find ("Sphere").transform.position = leapToWorld (righthand.Fingers [0].TipPosition);
			//debug.SetPosition (0, Vector3.zero);
			//debug.SetPosition (1, leapToWorld(lefthand.Fingers[0].TipPosition));
		}
	}

	void detectgrab(){
		if (grabbed == null) {
			//leapToWorld(lefthand.
		}
	}

	Vector3 leapToUnity(Leap.Vector v)
	{
		Vector3 result = new Vector3(0,0,0);
		result.x = -v.x/1000.0f;
		result.y = -v.z/1000.0f;
		result.z = v.y/1000.0f;
		return result;
	}

	Vector3 leapToWorld(Leap.Vector v){
		Camera c = leapspace.GetComponent<Camera> ();
		Matrix4x4 m = c.cameraToWorldMatrix;
		//Vector4 camori = m * new Vector4 (0, 0, 0, 1);
		Vector3 temp=leapToUnity (v);
		Vector4 cameravec = m * new Vector4 (temp.x, temp.y, temp.z, 0);
		Vector3 res = new Vector3 (cameravec[0], cameravec[1], cameravec[2]);
		res = res + leaporigin;
		return res;
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

	LineRenderer DrawLine(Vector3 start, Vector3 end,Color color,float width)
	{
		GameObject myline = new GameObject ();
		myline.transform.position = start;
		myline.AddComponent<LineRenderer> ();
		LineRenderer lr = myline.GetComponent<LineRenderer> ();
		lr.material = new Material (Shader.Find("Transparent/Diffuse"));
		lr.SetWidth (width, width);
		lr.positionCount = 2;
		lr.SetPosition (0, start);
		lr.SetPosition (1, end);
		return lr;
	}

}
