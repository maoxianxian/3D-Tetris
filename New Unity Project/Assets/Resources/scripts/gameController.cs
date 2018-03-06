using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class gameController{
	List<GameObject> fallingObs;
	int[ , , ] occupied;
	int groundsize;
	GameObject player;
	Vector3 xoffset = new Vector3 (0, 0, 0);
	Vector3 yoffset = new Vector3 (0, 0, 0);
	Vector3 zoffset = new Vector3 (0, 0, 0);//might need in future to expand cube
	Vector3 totaloffset = new Vector3 (0, -0.5f, 0);
	int numberOfPuzzle;

	public gameController(int size, GameObject playerOb, int numberOfP){
		groundsize = size;
		fallingObs=new List<GameObject>();
		occupied = new int[size, size, size];
		this.player = playerOb;
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
		setGrid (pos);
	}

	public void generatePuzzle(){
		System.Random rnd = new System.Random();
		int puzzleindex = rnd.Next (0, numberOfPuzzle);
		int x = rnd.Next (1, groundsize - 2);
		int y = rnd.Next (1, groundsize - 2);
		int z = rnd.Next (1, groundsize - 2);
		GameObject newpuzzle=GameObject.Instantiate(Resources.Load("prefabs/puzzle"+puzzleindex.ToString));
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

	bool setGrid(Vector3 cood){
		if (occupied [(int)cood.x, (int)cood.y, (int)cood.x] == 1) {
			return false;
		}
		occupied [(int)cood.x, (int)cood.y, (int)cood.x] = 1;
		return true;
	}

	bool checkGrid(Vector3 cood){
		if (occupied [(int)cood.x, (int)cood.y, (int)cood.x] == 1) {
			return true;
		}
		return false;
	}


}
