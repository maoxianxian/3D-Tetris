﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace gam {
    public class addBomb : MonoBehaviour {
        public Destroycub scr;
        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {

        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.name[0] == 'b'&& collision.gameObject.name[2] == 'n'&&handController.getctr.bomb==null&&
                handController.getctr.cub==null)
            {
				if (handController.getctr.isFist(handController.getctr.lefthand)&&Vector3.Dot(handController.getctr.leapspace.transform.forward,Vector3.down)>0.7f&&
					gameController.ctr.money>=30)
                {
					gameController.ctr.money -= 30; 
                    GameObject temp = GameObject.Instantiate(gameObject);
                    Destroy(temp.GetComponent<addBomb>());
                    temp.AddComponent(typeof(Destroycub));
                    handController.getctr.addBomb(temp);
                }
            }
        }
        void OnTriggerExit(Collider collision)
        {
        }
    }
}