using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace gam
{
    public class gameController
    {
        //int[,] puzzlecoord=new int[,]{{0,0,0},{0,0,1},{0,0,2},{0,1,0},
        /*	Vector3[] puzzlecoord={
                new Vector3(0,0,0),	new Vector3(0,0,1),	new Vector3(0,0,2),	new Vector3(0,1,0),				
                new Vector3(0,0,0),	new Vector3(0,0,1),	new Vector3(0,0,2),	new Vector3(0,1,1),				
                new Vector3(0,0,0),	new Vector3(0,0,1),	new Vector3(0,0,2),	new Vector3(0,0,3),	
                new Vector3(0,0,0),	new Vector3(0,0,1),	new Vector3(0,1,2),	new Vector3(0,1,1),				
                new Vector3(0,1,0),	new Vector3(0,0,0),	new Vector3(0,0,1),	new Vector3(0,1,1)
            };*/
        static int[,,] occupied;
        static int groundsize;
        GameObject player;
        Vector3 playercoord;
        Vector3 xoffset = new Vector3(0, 0, 0);
        Vector3 yoffset = new Vector3(0, 0, 0);
        Vector3 zoffset = new Vector3(0, 0, 0);//might need in future to expand cube
        Vector3 totaloffset = new Vector3(0, -0.5f, 0);
        int numberOfPuzzle;
        int playerid = 999;
        int puzzlecount = 0;
        int timePerGenerate = 6;//how long to generate a puzzle
        float generatetime;
        float movetime = 0;
        int timeperunit = 3;//time a cube spend on one unit


        //List<ObjectMover> movers;
        static List<puzzle> puzzles;
        public static gameController ctr;
        public gameController(int size, GameObject playerOb, int numberOfP)
        {
            puzzles = new List<puzzle>();
            groundsize = size;
            occupied = new int[size, size, size];
            player = playerOb;
            numberOfPuzzle = numberOfP;
            timePerGenerate = timeperunit * groundsize;
            generatetime = timePerGenerate-3;
            ctr = this;
        }

        public void CreateEnvironment()
        {
            //ground
            CreateCubeMat(Vector3.right, Vector3.forward, Vector3.zero - yoffset + totaloffset);
            //walls-left,forward,right,backward
            CreateCubeMat(Vector3.up, Vector3.forward, new Vector3(-0.5f, 0.5f, 0) - xoffset + totaloffset);
            CreateCubeMat(Vector3.up, Vector3.right, Vector3.forward * groundsize + new Vector3(0, 0.5f, -0.5f) + zoffset + totaloffset);
            CreateCubeMat(Vector3.up, Vector3.forward, Vector3.right * groundsize + new Vector3(-0.5f, 0.5f, 0) + xoffset + totaloffset);
            CreateCubeMat(Vector3.up, Vector3.right, new Vector3(0, 0.5f, -0.5f) - zoffset + totaloffset);
            //ceiling
            CreateCubeMat(Vector3.right, Vector3.forward, Vector3.up * groundsize + yoffset + totaloffset);
            //place player
            Vector3 pos = new Vector3(0, 0, 0);
            player.transform.position = pos;
            playercoord = pos;
            setGrid(pos, playerid);
        }

        public void generatePuzzle()
        {
            generatetime += Time.deltaTime;
            if (generatetime > timePerGenerate)
            {
                generatetime = 0;

                puzzlecount++;
                System.Random rnd = new System.Random();
                //int puzzleindex = rnd.Next(1, numberOfPuzzle + 1);
                int puzzleindex = puzzlecount % numberOfPuzzle + 1;
                int x = rnd.Next(0, groundsize);
                int z = rnd.Next(0, groundsize - 3);
                GameObject newpuzzle = (GameObject)GameObject.Instantiate(Resources.Load("prefabs/puzzle" + puzzleindex.ToString()));
                newpuzzle.transform.position = new Vector3(x, groundsize - 2, z);
                puzzle newp = new puzzle(newpuzzle, puzzlecount);
                for (int i = 0; i < newpuzzle.transform.childCount; i++)
                {
                    GameObject cube = newpuzzle.transform.GetChild(i).gameObject;
                    Cube newcub = new Cube(cube, puzzlecount);
                    newp.addCube(newcub);
                }
                //fallingPuzzls.Add(newp);
                puzzles.Add(newp);
                newpuzzle.name = puzzlecount.ToString();
            }
        }

        public void moveplayer(Vector3 dir)
        {
            if (valid(player.transform.position + dir)&&getGrid(player.transform.position+dir)==0)
            {
                unsetGrid(player.transform.position);
                player.transform.position += dir;
                setGrid(player.transform.position, playerid);
                playercoord = player.transform.position;
            }
        }
        public bool BeginFallPuzzle()
        {
            bool gene = true;
            for (int i = puzzles.Count - 1; i >= 0; i--)
            {//foreach puzzle
                if (puzzles[i].startMove(Vector3.down))
                {
                    gene = false;
                }
            }
            if (gene)
            {
                generatetime = timePerGenerate;
            }
            return true;
        }

        public void moveFallingPuzzle()
        {
            movetime += Time.deltaTime;
            if (movetime > timeperunit)
            {
                movetime = 0;
                if (checkMat())
                {
                    foreach(puzzle p in puzzles)
                    {
                        p.destroybuttom();
                    }
                }
                BeginFallPuzzle();
            }
            foreach (puzzle m in puzzles)
            {
                m.Update();
            }
        }

        public void expandgrid() {
            while (puzzles.Count != 0)
            {
                removepuzzle(0);
            }
            groundsize = groundsize++;
            occupied = new int[groundsize, groundsize, groundsize];
            timePerGenerate = timeperunit * groundsize;
            generatetime = timePerGenerate - 3;
        }
        public void smallergrid()
        {
            while (puzzles.Count != 0)
            {
                removepuzzle(0);
            }
            groundsize = groundsize--;
            occupied = new int[groundsize, groundsize, groundsize];
            timePerGenerate = timeperunit * groundsize;
            generatetime = timePerGenerate - 3;
        }
        public void removepuzzle(int i)
        {
            puzzles[i].destroy();
            puzzles.RemoveAt(i);
        }
        public void Update()
        {
            generatePuzzle();
            moveFallingPuzzle();
        }

        public int[,,] getGrid()
        {
            return occupied;
        }

        bool checkMat()
        {
            for (int x = 0; x < groundsize; x++)
            {
                for (int z = 0; z < groundsize; z++)
                {
                    if (occupied[x, 0, z] == 0 || occupied[x, 0, z] == playerid)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        void CreateCubeMat(Vector3 x, Vector3 y, Vector3 ori)
        {
            for (int i = 0; i < groundsize; i++)
            {
                for (int j = 0; j < groundsize; j++)
                {
                    GameObject groundcub = (GameObject)GameObject.Instantiate(Resources.Load("prefabs/groundCube"));
                    groundcub.transform.position = ori + i * x + j * y;
                    groundcub.transform.up = Vector3.Cross(x, y);
                }
            }
        }

        public static Vector3 WorldToCube(Vector3 WorldCoord)
        {
            float x = Mathf.Floor(WorldCoord.x + 0.5f);
            float y = Mathf.Floor(WorldCoord.y + 0.5f);
            float z = Mathf.Floor(WorldCoord.z + 0.5f);
            return new Vector3(x, y, z);
        }

        public static puzzle GetPuzzle(int i)
        {
            return puzzles[i-1];
        }
        public static int getGrid(Vector3 cood)
        {
            return occupied[(int)(cood.x), (int)(cood.y), (int)(cood.z)];
        }

        public static bool setGrid(Vector3 cood, int val)
        {
            if (getGrid(cood) == val)
            {
                return false;
            }
            occupied[(int)(cood.x), (int)(cood.y), (int)(cood.z)] = val;
            return true;
        }

        public static bool unsetGrid(Vector3 cood)
        {
            if (getGrid(cood) == 0)
            {
                return false;
            }
            occupied[(int)cood.x, (int)cood.y, (int)cood.z] = 0;
            return true;
        }

        public static void printGrid()
        {
            for (int i = 0; i < groundsize; i++)
            {
                for (int j = 0; j < groundsize; j++)
                {
                    for (int k = 0; k < groundsize; k++)
                    {
                        if (occupied[i, j, k] != 0)
                        {
                            Debug.Log("coord: " + i.ToString() + " " + j.ToString() + " " + k.ToString() + " val: " + occupied[i, j, k]);
                        }
                    }
                }
            }
        }

        public static bool valid(Vector3 cood)
        {
            for (int i = 0; i < 3; i++)
            {
                if (cood[i] < 0 || cood[i] >= occupied.GetLength(1))
                {
                    return false;
                }
            }
            return true;
        }
    }
}