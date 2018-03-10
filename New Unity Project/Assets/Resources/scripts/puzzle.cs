using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam {
    public class puzzle {
        GameObject puz;
        int id;
        List<Cube> cubes;
        public puzzle(GameObject obj, int ind)
        {
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
            return true;
        }

        public void Update()
        {
            foreach(Cube c in cubes)
            {
                c.Update();
            }
        }
    }
}
