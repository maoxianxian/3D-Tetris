using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam
{
    public class switchmode : MonoBehaviour
    {
        Material orimat;
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
            dehighlight();
        }

        void highlight()
        {
            orimat = GetComponent<Renderer>().material;
            GetComponent<Renderer>().material= Resources.Load("materials/highlightmat", typeof(Material)) as Material;
        }
        void dehighlight()
        {
            GetComponent<Renderer>().material = orimat;
        }
    }
}
