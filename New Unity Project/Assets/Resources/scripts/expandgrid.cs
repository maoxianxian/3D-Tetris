using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam
{
    public class expandgrid : MonoBehaviour
    {
        Material orimat;
        float t = 0;
        // Use this for initialization
        void Start()
        {
            orimat = GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            t += Time.deltaTime;

        }
        void OnTriggerEnter(Collider collision)
        {
            if (t > 1)
            {
                if (collision.gameObject.name[0] == 'b')
                {
                    gameController.ctr.expandgrid();
                    highlight();
                    t = 0;
                }
            }
        }
        void OnTriggerExit(Collider collision)
        {
            dehighlight();
        }

        void highlight()
        {
            GetComponent<Renderer>().material = Resources.Load("materials/highlightmat", typeof(Material)) as Material;
        }
        void dehighlight()
        {
            GetComponent<Renderer>().material = orimat;
        }
    }
}