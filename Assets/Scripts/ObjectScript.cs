using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{

    public GameObject Imprint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Imprint")
        { if(collision.transform.name == Imprint.name)
            {
                this.GetComponent<NearInteractionGrabbable>().enabled = false;
                this.GetComponent<ObjectManipulator>().enabled = false;
                print("Correct Imprint");
            } else
            {
                // temporary, hits another Imprint
                print("Wrong Imprint");
            }
        }
    }
}
