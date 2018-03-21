using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam
{
    public class randomrot : MonoBehaviour
    {
        float t = 0;
        Material orimat;
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
            int ite = 0;
            if (t > 0.5f)
            {
                if (collision.gameObject.name[0] == 'b')
                {
                    while (!gameController.ctr.randomRotCur()&&ite<20)
                    {
                        ite++;
                    }
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