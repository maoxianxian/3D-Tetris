using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace gam
{
    public class Destroycub : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (!gameController.valid(gameController.WorldToCube(transform.position)))
            {
                GameObject.Destroy(handController.getctr.bomb);
                handController.getctr.bomb = null;
                handController.getctr.release = false;
            }
        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.name[0] == 'C' && handController.getctr.bomb != null && handController.getctr.release == true)
            {
                GameObject cub = collision.gameObject;
                gameController.ctr.destroycub(Int32.Parse(cub.transform.parent.name), cub.transform.position);
            }
        }
        void OnTriggerExit(Collider collision)
        {
        }
    }
}