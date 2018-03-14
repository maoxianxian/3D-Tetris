using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace gam {
    public class addBomb : MonoBehaviour {
        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {

        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.name[0] == 'b'&& collision.gameObject.name[2] == 'n'&&handController.getctr.bomb==null)
            {
                GameObject temp =GameObject.Instantiate(gameObject);
                Destroy(temp.GetComponent<addBomb>());
                handController.getctr.addBomb(temp);
            }
        }
        void OnTriggerExit(Collider collision)
        {
        }
    }
}