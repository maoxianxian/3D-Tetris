using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace gam
{
    public class Cube
    {
        GameObject cub;
        Vector3 coord;
        int id;
        public Cube(GameObject obj, int index)
        {
            id = index;
            cub = obj;
            coord = gameController.WorldToCube(obj.transform.position);
            gameController.setGrid(coord,index);
        }

        public bool checktar(Vector3 tar)
        {
            Vector3 targetcoord = coord + tar;

            if (!gameController.valid(targetcoord))
            {
                return false;
            }
            if (gameController.getGrid(targetcoord)!=0&& gameController.getGrid(targetcoord)!=id)
            {
                return false;
            }
            return true;
        }

        public void startMove(Vector3 Dir)
        {
            Vector3 targetcoord = coord + Dir;
            gameController.unsetGrid(coord);
            gameController.setGrid(targetcoord, id);
            coord = targetcoord;
        }

        public bool checkrotate(Vector3 axis, Vector3 gridpoint)
        {
            Debug.Log("checking");
            GameObject temp = (GameObject)GameObject.Instantiate(Resources.Load("prefabs/rotator"));
            temp.transform.position = coord;
            temp.transform.RotateAround(gridpoint, axis, 90.0f);
            Vector3 targetcor = temp.transform.position;
            Debug.Log(targetcor);
            if (!gameController.valid(targetcor))
            {
                return false;
            }
            if (gameController.getGrid(targetcor) != 0 && gameController.getGrid(targetcor) != id)
            {
                return false;
            }
            return true;
        }

        public void rotateAround(Vector3 axis, Vector3 gridpoint)
        {
            //Debug.Log("axis " + axis.ToString());
            //Debug.Log(gridpoint.ToString());
            GameObject temp =(GameObject)GameObject.Instantiate(Resources.Load("prefabs/rotator"));
            temp.transform.position = coord;
            temp.transform.RotateAround(gridpoint, axis, 90.0f);
            gameController.unsetGrid(coord);
            coord = temp.transform.position;
            gameController.setGrid(coord, id);
        }

        public void highlight(Material mat)
        {
            cub.GetComponent<Renderer>().material = mat;
        }
    }
}