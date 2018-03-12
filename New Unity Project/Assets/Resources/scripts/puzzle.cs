using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam {
    public class puzzle {
        GameObject puz;
        int id;
        public List<Cube> cubes;
        Vector3 coord;
        Vector3 movedir;
        Material puzzlemat;
        public puzzle(GameObject obj, int ind)
        {
            puzzlemat = obj.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
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
            foreach (Cube c in cubes)
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
                if (!c.checkrotate(axis, coord))
                {
                    return false;
                }
            }
            foreach (Cube c in cubes)
            {
                c.rotateAround(axis, coord);
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
                dehighlight();
            }
        }

        public void highlight()
        {
            Material mat = Resources.Load("materials/highlightmat", typeof(Material)) as Material;
            foreach (Cube c in cubes)
            {
                c.highlight(mat);
            }
        }

        public void dehighlight()
        {
            foreach (Cube c in cubes)
            {
                c.highlight(puzzlemat);
            }
        }

        public void destroybuttom()
        {
            for(int i = cubes.Count - 1; i >= 0; i--)
            {
                if (cubes[i].coord.y == 0)
                {
                    Cube temp = cubes[i];
                    gameController.unsetGrid(temp.coord);
                    cubes.RemoveAt(i);
                    GameObject.Destroy(temp.cub);
                }
            }
        }

        public void destroy()
        {
            for (int i = cubes.Count - 1; i >= 0; i--)
            {
                Cube temp = cubes[i];
                gameController.unsetGrid(temp.coord);
                cubes.RemoveAt(i);
                GameObject.Destroy(temp.cub);
            }
        }

        public void destroy(Vector3 coord)
        {

        }
    }
}
