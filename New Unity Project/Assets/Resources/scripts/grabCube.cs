using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam
{

    public class grabCube : MonoBehaviour
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
            if (collision.gameObject.name[0] == 'b' && collision.gameObject.name[2] == 'n' && handController.getctr.bomb == null&&
                handController.getctr.cub==null)
            {
				if (handController.getctr.isFist(handController.getctr.lefthand)&&Vector3.Dot(handController.getctr.leapspace.transform.forward,Vector3.down)>0.7f)
                {
                    handController.getctr.addCub();
                }
            }
        }
        void OnTriggerExit(Collider collision)
        {
        }
    }
}