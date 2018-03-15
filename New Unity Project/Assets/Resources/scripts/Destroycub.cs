using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace gam
{
    public class Destroydub : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.name[0] == 'C' && handController.getctr.bomb != null && handController.getctr.release == true)
            {
                Debug.Log(collision.gameObject.name);
                GameObject cub = collision.gameObject;
                gameController.ctr.destroycub(Int32.Parse(cub.transform.parent.name), cub.transform.position);
            }
        }
        void OnTriggerExit(Collider collision)
        {
        }
    }
}