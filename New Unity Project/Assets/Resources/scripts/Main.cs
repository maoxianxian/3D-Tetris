using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
	int groundsize=6;
	GameObject player;
	// Use this for initialization
	void Start () {
		CreateEnvironment ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CreateEnvironment(){
		
		//ground
		CreateCubeMat(Vector3.right, Vector3.forward,Vector3.zero+new Vector3(0,-0.1f,0));
		//walls-left,forward,right,backward
		CreateCubeMat (Vector3.up, Vector3.forward, new Vector3 (-0.6f, 0.5f, 0));
		CreateCubeMat (Vector3.up, Vector3.right, Vector3.forward * groundsize + new Vector3 (0, 0.5f, -0.4f));
		CreateCubeMat (Vector3.up, Vector3.forward, Vector3.right * groundsize + new Vector3 (-0.4f, 0.5f, 0));
		CreateCubeMat (Vector3.up, Vector3.right, new Vector3 (0, 0.5f, -0.6f));
		//ceiling
		CreateCubeMat (Vector3.right, Vector3.forward, Vector3.up * groundsize+new Vector3 (0, 0.1f, 0));
	}

	void CreateCubeMat(Vector3 x, Vector3 y, Vector3 ori){
		for (int i = 0; i < groundsize; i++) {
			for (int j = 0; j < groundsize; j++) {
				GameObject groundcub = (GameObject)Instantiate (Resources.Load ("prefabs/groundCube"));
				groundcub.transform.position = ori + i * x + j * y;
				groundcub.transform.up = Vector3.Cross (x, y);
			}
		}
	}

}
