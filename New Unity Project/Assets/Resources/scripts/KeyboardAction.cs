using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam
{
    public class KeyboardAction : MonoBehaviour
    {
        public static UnityEngine.UI.Text nametxt;
        public static string username="";
        // Use this for initialization
        void Start()
        {
            nametxt = GameObject.Find("name").GetComponent<UnityEngine.UI.Text>();
        }

        // Update is called once per frame
        void Update()
        {
            GameObject leap = handController.getctr.leapspace;
            nametxt.transform.position = leap.transform.position + 1.5f*leap.transform.forward+0.3f*leap.transform.up;
            nametxt.transform.forward = leap.transform.forward;
            transform.position = leap.transform.position+0.4f*leap.transform.forward-0.2f*leap.transform.up;
            //transform.forward = Vector3.Normalize(-leap.transform.up+leap.transform.forward);
            transform.LookAt(transform.position-Vector3.up);
        }
    }
}