using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gam{
    public class smallergrid : MonoBehaviour {
        Material orimat;
        float t;
        float preestime;
        int coliitem = 0;
        float presstime = 0;
        bool pressing = false;
        // Use this for initialization
        void Start() {
            orimat = GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update() {
            t += Time.deltaTime;
            if (pressing)
            {
                presstime += Time.deltaTime;
            }
            if (presstime > 0.5f)
            {
                pressing = false;
                presstime = 0;
                gameController.ctr.smallergrid();
            }
        }
        void OnTriggerEnter(Collider collision)
        {
            if (t > 1)
            {
                if (collision.gameObject.name[0] == 'b')
                {
                    coliitem++;
                    highlight();
                    t = 0;
                    pressing = true;
                }
            }
        }
        void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.name[0] == 'b')
            {
                coliitem--;
                presstime = 0;
                pressing = false;
                dehighlight();
            }
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