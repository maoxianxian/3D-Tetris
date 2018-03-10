using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectMover {
	GameObject obj;
	int[,,] grid;
	int step = 0;
	int totalStep=100;
	Vector3 direction;
	public ObjectMover(GameObject objl, int[,,] gridl){
		obj = objl;
		grid = gridl;
	}

	public bool startMove(Vector3 dir){
		direction = dir;
		if (obj.tag == "puzzle") {
			for (int j = 0; j <obj.transform.childCount; j++) {//foreach cube in puzzle
				GameObject cube = obj.transform.GetChild (j).gameObject;
				Vector3 cubecoor = WorldToCube (cube.transform.position);
				Vector3 targetcoord = cubecoor + dir;
				if (!valid (targetcoord)) {
					return false;
				}
				unsetGrid (cubecoor);
				setGrid (targetcoord, Int32.Parse (obj.name));
			}
		}
		return true;
	}

	public void Update(){
		if (step < totalStep) {
			step++;
			obj.transform.position += direction/(float)totalStep;
		} else {
			step++;
		}
	}
	Vector3 WorldToCube(Vector3 WorldCoord){
		float x = Mathf.Floor (WorldCoord.x + 0.5f);
		float y = Mathf.Floor (WorldCoord.y + 0.5f);
		float z = Mathf.Floor (WorldCoord.z + 0.5f);
		return new Vector3 (x, y, z);
	}

	int getGrid(Vector3 cood){
		return grid [(int)(cood.x), (int)(cood.y), (int)(cood.z)];
	}

	bool setGrid(Vector3 cood, int val){
		if (getGrid(cood) == val) {
			return false;
		}
		grid [(int)(cood.x), (int)(cood.y), (int)(cood.z)] = val;
		return true;
	}

	bool unsetGrid(Vector3 cood){
		if (getGrid(cood) == 0) {
			return false;
		}
		grid [(int)cood.x, (int)cood.y, (int)cood.z] = 0;
		return true;
	}

	bool valid(Vector3 cood){
		for (int i = 0; i < 3; i++) {
			if (cood [i] < 0 || cood [i] == grid.GetLength (1)) {
				return false;
			}
		}
		return true;
	}
}
