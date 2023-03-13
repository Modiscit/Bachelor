using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{

    public GameObject Imprint;
    public bool placed;

    // Start is called before the first frame update
    void Start()
    {
        // should be false when validate button works correctly
        placed = true;
        Interactable();
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
                placed = false;
                Interactable();
                GameObject.Find("Parameters").GetComponent<ParametersScript>().CheckEnd();
                print("Correct Imprint");
            } else
            {
                // temporary, hits another Imprint
                print("Wrong Imprint");
            }
        }
    }

    public void Interactable(){
        this.GetComponent<NearInteractionGrabbable>().enabled = placed;
        this.GetComponent<ObjectManipulator>().enabled = placed;
    }
}
