using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam
{
    public class onCol : MonoBehaviour
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
            if (collision.gameObject.name[0] == 'b')
            {
                if (handController.getctr.switchmode())
                {
                    highlight();
                }
            }
        }
        void OnTriggerExit(Collider collision)
        {
            
        }

        void highlight()
        {

        }
        void dehighlight()
        {

        }
    }
}
