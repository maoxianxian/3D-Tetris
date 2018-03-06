using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
	int groundsize=6;
	gameController controller;
	GameObject player;
	// Use this for initialization
	void Start () {
		//init player
		player = GameObject.FindGameObjectWithTag ("Player");
		//init controller
		controller = new gameController (groundsize, player);
		controller.CreateEnvironment ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}



}
