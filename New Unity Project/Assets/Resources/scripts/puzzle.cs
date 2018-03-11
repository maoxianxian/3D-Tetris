using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam {
    public class puzzle {
        GameObject puz;
        int id;
        List<Cube> cubes;
        Vector3 coord;
        Vector3 movedir;
        public puzzle(GameObject obj, int ind)
        {
            coord = obj.transform.position;
            cubes = new List<Cube>();
            puz = obj;
            id = ind;
        }

        public void addCube(Cube cub)
        {
            cubes.Add(cub);
        }

        public bool startMove(Vector3 dir)
        {
            foreach(Cube c in cubes)
            {
                if (!c.checktar(dir))
                {
                    return false;
                }
            }
            foreach(Cube c in cubes)
            {
                c.startMove(dir);
            }
            movedir += dir;
            coord = coord + dir;
            return true;
        }

        public bool rotate(Vector3 axis)
        {
            foreach (Cube c in cubes)
            {
                if (!c.checkrotate(axis,puz.transform.position,coord))
                {
                    return false;
                }
            }
            Debug.Log("start rotate");
            foreach (Cube c in cubes)
            {
                c.rotateAround(axis, puz.transform.position, coord);
            }
            puz.transform.RotateAround(puz.transform.position,axis,90);
            return true;
        }
        public void Update()
        {
            if (movedir.x != 0)
            {
                puz.transform.position += Vector3.Normalize(new Vector3(movedir.x, 0, 0)) * 0.01f;
                movedir -= Vector3.Normalize(new Vector3(movedir.x, 0, 0)) * 0.01f;
            }
            if (movedir.y != 0)
            {
                puz.transform.position += Vector3.Normalize(new Vector3(0, movedir.y, 0)) * 0.01f;
                movedir -= Vector3.Normalize(new Vector3(0, movedir.y, 0)) * 0.01f;
            }
            if (movedir.z != 0)
            {
                puz.transform.position += Vector3.Normalize(new Vector3(0, 0, movedir.z)) * 0.01f;
                movedir -= Vector3.Normalize(new Vector3(0, 0, movedir.z)) * 0.01f;
            }
            if (movedir == Vector3.zero)
            {
                puz.transform.position = coord;
            }
        }
    }
}
