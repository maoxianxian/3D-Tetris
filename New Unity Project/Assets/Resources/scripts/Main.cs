using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam
{
    public class Main : MonoBehaviour
    {
        int groundsize = 6;
        float generatetime = 0;
        float movetime = 0;
        int timePerGenerate = 6;//how long to generate a puzzle
        int numberOfPuzzle = 5;
        int timeperunit = 3;//time a cube spend on one unit
        int total = 0;
        gameController controller;
        GameObject player;
        handController hands;
        // Use this for initialization
        void Start()
        {
            //init player
            player = GameObject.FindGameObjectWithTag("Player");
            //init controller
            controller = new gameController(groundsize, player, numberOfPuzzle);
            controller.CreateEnvironment();
            hands = new handController(controller);
        }

        // Update is called once per frame
        void Update()
        {
            generatetime += Time.deltaTime;
            movetime += Time.deltaTime;
            if (generatetime > timePerGenerate)
            {
                total++;
                generatetime = 0;
                controller.generatePuzzle();
            }
            if (movetime > timeperunit)
            {
                movetime = 0;
                controller.BeginFallPuzzle();
            }
            controller.moveFallingPuzzle();

            hands.connectToHands();
            hands.moveobj();
        }
    }
}
