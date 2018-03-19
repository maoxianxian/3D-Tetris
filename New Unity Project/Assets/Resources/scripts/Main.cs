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
		public UnityEngine.UI.Text point;
        public static bool gamestart = false;
        public static string username;
        // Use this for initialization
        void Start()
        {
            //init player
            player = GameObject.FindGameObjectWithTag("Player");
            //init controller
            controller = new gameController(groundsize, player, numberOfPuzzle,gg);
            controller.CreateEnvironment();
            hands = new handController(controller, sphere1, sphere2, sphere3, sphere4, xtxt, ytxt, ztxt, wtxt);
			CreateBoard ();
        }

        // Update is called once per frame
        void Update()
        {
            if (gamestart)
            {
                controller.Update();
                hands.connectToHands();
                hands.Update();
                int size = gameController.groundsize;
                point.transform.position = new Vector3(size / 2, size - 1, size / 2);
                point.transform.forward = Vector3.up;
                point.text = "money:" + System.Environment.NewLine + gameController.ctr.money;
            }
        }
		void CreateBoard(){
			string[] rows = { "QWERTYUIOP", "ASDFGHJKL", "ZXCVBNM" };
			GameObject keyboard = GameObject.FindGameObjectWithTag ("keyboard");
			keyboard.transform.position = new Vector3(0.7f,0,4);
			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < rows [i].Length; j++) {
					GameObject key = (GameObject)GameObject.Instantiate (Resources.Load ("prefabs/Ka"));
                    key.name = "K" + rows[i].Substring(j, 1);
					key.transform.SetParent(keyboard.transform);
					key.transform.position = new Vector3 (0.8f*i, 0.4f, 0.8f*j+0.3f);
					key.transform.GetChild (0).gameObject.GetComponent<UnityEngine.UI.Text> ().text = rows [i].Substring (j, 1);
				}
			}
            keyboard.transform.localScale/=12.0f;
		}
    }
}
