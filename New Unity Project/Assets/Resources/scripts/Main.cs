using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam
{
    public class Main : MonoBehaviour
    {
        int groundsize = 6;
        int numberOfPuzzle = 5;
        gameController controller;
        GameObject player;
        handController hands;
        public GameObject sphere1;
        public GameObject sphere2;
        public GameObject sphere3;
        public GameObject sphere4;
        public UnityEngine.UI.Text xtxt;
        public UnityEngine.UI.Text ytxt;
        public UnityEngine.UI.Text ztxt;
        public UnityEngine.UI.Text wtxt;
        public UnityEngine.UI.Text gg;
        // Use this for initialization
        void Start()
        {
            //init player
            player = GameObject.FindGameObjectWithTag("Player");
            //init controller
            controller = new gameController(groundsize, player, numberOfPuzzle,gg);
            controller.CreateEnvironment();
            hands = new handController(controller, sphere1, sphere2, sphere3, sphere4, xtxt, ytxt, ztxt, wtxt);
        }

        // Update is called once per frame
        void Update()
        {
            controller.Update();
            hands.connectToHands();
            hands.moveobj();
        }
    }
}
