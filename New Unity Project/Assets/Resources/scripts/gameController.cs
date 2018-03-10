using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class gameController{
	//int[,] puzzlecoord=new int[,]{{0,0,0},{0,0,1},{0,0,2},{0,1,0},
/*	Vector3[] puzzlecoord={
		new Vector3(0,0,0),	new Vector3(0,0,1),	new Vector3(0,0,2),	new Vector3(0,1,0),				
		new Vector3(0,0,0),	new Vector3(0,0,1),	new Vector3(0,0,2),	new Vector3(0,1,1),				
		new Vector3(0,0,0),	new Vector3(0,0,1),	new Vector3(0,0,2),	new Vector3(0,0,3),	
		new Vector3(0,0,0),	new Vector3(0,0,1),	new Vector3(0,1,2),	new Vector3(0,1,1),				
		new Vector3(0,1,0),	new Vector3(0,0,0),	new Vector3(0,0,1),	new Vector3(0,1,1)
	};*/
	List<GameObject> fallingObs;
	int currentStep = 0;
	int totalStep=100;
	int[ , , ] occupied;
	int groundsize;
	GameObject player;
	Vector3 playercoord;
	Vector3 xoffset = new Vector3 (0, 0, 0);
	Vector3 yoffset = new Vector3 (0, 0, 0);
	Vector3 zoffset = new Vector3 (0, 0, 0);//might need in future to expand cube
	Vector3 totaloffset = new Vector3 (0, -0.5f, 0);
	int numberOfPuzzle;
	int playerid=999;
	int puzzlecount=0;
	List<ObjectMover> movers;

	public gameController(int size, GameObject playerOb, int numberOfP){
		movers = new List<ObjectMover> ();
		groundsize = size;
		fallingObs = new List<GameObject> ();
		occupied = new int[size, size, size];
		player = playerOb;
		numberOfPuzzle = numberOfP;
	}

	public void CreateEnvironment(){
		//ground
		CreateCubeMat (Vector3.right, Vector3.forward, Vector3.zero - yoffset + totaloffset);
		//walls-left,forward,right,backward
		CreateCubeMat (Vector3.up, Vector3.forward, new Vector3 (-0.5f, 0.5f, 0) - xoffset + totaloffset);
		CreateCubeMat (Vector3.up, Vector3.right, Vector3.forward * groundsize + new Vector3 (0, 0.5f, -0.5f) + zoffset + totaloffset);
		CreateCubeMat (Vector3.up, Vector3.forward, Vector3.right * groundsize + new Vector3 (-0.5f, 0.5f, 0) + xoffset + totaloffset);
		CreateCubeMat (Vector3.up, Vector3.right, new Vector3 (0, 0.5f, -0.5f) - zoffset + totaloffset);
		//ceiling
		CreateCubeMat (Vector3.right, Vector3.forward, Vector3.up * groundsize + yoffset + totaloffset);
		//place player
		Vector3 pos = new Vector3 (groundsize / 2, 0, groundsize / 2);
		player.transform.position = pos;
		playercoord = pos;
		setGrid (pos, playerid);
	}

	public void generatePuzzle(){
		puzzlecount++;
		System.Random rnd = new System.Random();
		int puzzleindex = rnd.Next (1, numberOfPuzzle+1);
		int x = rnd.Next (0, groundsize);
		int z = rnd.Next (0, groundsize - 3);
		GameObject newpuzzle=(GameObject)GameObject.Instantiate(Resources.Load("prefabs/puzzle"+puzzleindex.ToString()));
		newpuzzle.transform.position = new Vector3 (x, groundsize - 2, z);
		for (int i = 0; i < newpuzzle.transform.childCount; i++) {
			GameObject cube = newpuzzle.transform.GetChild (i).gameObject;
			Vector3 cubecoord = WorldToCube (cube.transform.position);
			setGrid (cubecoord, puzzlecount);
		}
		fallingObs.Add (newpuzzle);
		newpuzzle.name = puzzlecount.ToString();
	}

	public bool BeginMovePuzzle(){
		movers.Clear ();
		for (int i = fallingObs.Count - 1; i >= 0; i--) {//foreach puzzle
			bool puzzlemovale = true;
			for (int j = fallingObs [i].transform.childCount - 1; j >= 0; j--) {//foreach cube in puzzle
				GameObject cube = fallingObs [i].transform.GetChild (j).gameObject;
				Vector3 cubecoor = WorldToCube (cube.transform.position);
				if (cubecoor.y > 0) {
					Vector3 targetcoord = cubecoor + Vector3.down;
					//if (targetcoord == playercoord) {//hit player
						//return false;
					//}
					if (!(getGrid (targetcoord) == 0) && !(getGrid (targetcoord) == getGrid (cubecoor))) {//cube not movable
						puzzlemovale = false;
					}
				} else {//puzzle not movable
					puzzlemovale = false;
				}
			}
			if (puzzlemovale) {
				ObjectMover mover = new ObjectMover (fallingObs [i],occupied);
				mover.startMove (new Vector3 (0, -1, 0));
				movers.Add (mover);
			} else {
				fallingObs.Remove (fallingObs [i]);
			}
		}
		currentStep = 0;
		return true;
	}

	public void moveFallingPuzzle(){
		if (currentStep < totalStep) {
			currentStep++;
			foreach (ObjectMover m in movers) {
				m.Update ();
			}
		} else if (currentStep == totalStep) {
			currentStep++;
			if (checkMat ()) {
			}
		}
	}

	public void addMover(ObjectMover mover){
		movers.Add (mover);
	}

	public int[,,] getGrid(){
		return occupied;
	}

	bool checkMat(){
		for (int x = 0; x < groundsize; x++) {
			for (int z = 0; z < groundsize; z++) {
				if (occupied [x, 0, z] == 0 || occupied [x, 0, z] == playerid) {
					return false;
				}
			}
		}
		return true;
	}

	void CreateCubeMat(Vector3 x, Vector3 y, Vector3 ori){
		for (int i = 0; i < groundsize; i++) {
			for (int j = 0; j < groundsize; j++) {
				GameObject groundcub = (GameObject)GameObject.Instantiate (Resources.Load ("prefabs/groundCube"));
				groundcub.transform.position = ori + i * x + j * y;
				groundcub.transform.up = Vector3.Cross (x, y);
			}
		}
	}

	Vector3 WorldToCube(Vector3 WorldCoord){
		float x = Mathf.Floor (WorldCoord.x + 0.5f);
		float y = Mathf.Floor (WorldCoord.y + 0.5f);
		float z = Mathf.Floor (WorldCoord.z + 0.5f);
		return new Vector3 (x, y, z);
	}

	int getGrid(Vector3 cood){
		return occupied [(int)(cood.x), (int)(cood.y), (int)(cood.z)];
	}

	bool setGrid(Vector3 cood, int val){
		if (getGrid(cood) == val) {
			return false;
		}
		occupied [(int)(cood.x), (int)(cood.y), (int)(cood.z)] = val;
		return true;
	}

	bool unsetGrid(Vector3 cood){
		if (getGrid(cood) == 0) {
			return false;
		}
		occupied [(int)cood.x, (int)cood.y, (int)cood.z] = 0;
		return true;
	}

	void printGrid(){
		for (int i = 0; i < groundsize; i++) {
			for (int j = 0; j < groundsize; j++) {
				for (int k = 0; k < groundsize; k++) {
					//Debug.Log (occupied [i,j,k]);
					if (occupied [i, j, k] != 0) {
						Debug.Log ("coord: "+i.ToString () + " " + j.ToString () + " " + k.ToString ()+" val: "+occupied [i, j, k]);
					}
				}
			}
		}
	}
}
