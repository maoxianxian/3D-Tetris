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
        Vector3 movedir = Vector3.zero;
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
            movedir += Dir;
            Vector3 targetcoord = coord + Dir;
            gameController.unsetGrid(coord);
            gameController.setGrid(targetcoord, id);
            coord = targetcoord;
        }

        public void Update()
        {
            if (movedir.x != 0)
            {
                cub.transform.position += Vector3.Normalize(new Vector3(movedir.x, 0, 0))*0.01f;
                movedir -= Vector3.Normalize(new Vector3(movedir.x, 0, 0)) * 0.01f;
            }
            if (movedir.y != 0)
            {
                cub.transform.position += Vector3.Normalize(new Vector3(0, movedir.y, 0)) * 0.01f;
                movedir -= Vector3.Normalize(new Vector3(0, movedir.y, 0)) * 0.01f;
            }
            if (movedir.z != 0)
            {
                cub.transform.position += Vector3.Normalize(new Vector3(0, 0, movedir.z)) * 0.01f;
                movedir -= Vector3.Normalize(new Vector3(0, 0, movedir.z)) * 0.01f;
            }
        }
    }
}