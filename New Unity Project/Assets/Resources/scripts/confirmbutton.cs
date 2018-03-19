using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam
{
    public class confirmbutton : MonoBehaviour
    {
        int coliitem = 0;
        Material orimat;
        bool pressing = false;
        float presstime = 0;
        // Use this for initialization
        void Start()
        {
            orimat = GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            if (pressing)
            {
                presstime += Time.deltaTime;
            }
            if (presstime > 0.5f)
            {
                pressing = false;
                presstime = 0;
                Main.gamestart = true;
                Main.username = KeyboardAction.username;
                GameObject.Destroy(GameObject.Find("Canvas2"));
            }
        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.name[0] == 'b')
            {
                coliitem++;
                highlight();
                pressing = true;
            }
        }
        void OnTriggerExit(Collider collision)
        {
            coliitem--;
            if (coliitem == 0)
            {
                presstime = 0;
                pressing = false;
                dehighlight();
            }
            if (coliitem < 0)
            {
                coliitem = 0;
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