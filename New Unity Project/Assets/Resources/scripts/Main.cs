using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam
{
    public class Main : MonoBehaviour
    {
        int groundsize = 6;
        float movetime = 0;
        int numberOfPuzzle = 5;
        int timeperunit = 3;//time a cube spend on one unit
        gameController controller;
        GameObject player;
        handController hands;
        public GameObject xsphere;
        public GameObject ysphere;
        public GameObject zsphere;
        public UnityEngine.UI.Text xtxt;
        public UnityEngine.UI.Text ytxt;
        public UnityEngine.UI.Text ztxt;
        // Use this for initialization
        void Start()
        {
            //init player
            player = GameObject.FindGameObjectWithTag("Player");
            //init controller
            controller = new gameController(groundsize, player, numberOfPuzzle);
            controller.CreateEnvironment();
            hands = new handController(controller, xsphere, ysphere, zsphere, xtxt, ytxt, ztxt);
        }

        // Update is called once per frame
        void Update()
        {
            movetime += Time.deltaTime;
            controller.generatePuzzle();
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
