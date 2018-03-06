using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
	int groundsize = 6;
	float timeinterval = 0;
	int targetTime = 6;//how long to generate a puzzle
	int numberOfPuzzle=5;
	gameController controller;
	GameObject player;
	// Use this for initialization
	void Start () {
		//init player
		player = GameObject.FindGameObjectWithTag ("Player");
		//init controller
		controller = new gameController (groundsize, player, numberOfPuzzle);
		controller.CreateEnvironment ();
	}
	
	// Update is called once per frame
	void Update () {
		timeinterval += Time.deltaTime;
		if (timeinterval > targetTime) {
			timeinterval = 0;
			controller.generatePuzzle ();
		}
	}



}
