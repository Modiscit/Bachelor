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
        // checks if the collision is with an imprint
        if(collision.transform.tag == "Imprint")
        {   // hits the right imprint
            if(collision.transform.name == Imprint.name)
            {
                // stops interaction
                placed = false;
                Interactable();
                // stops movement
                this.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                // checks if all objects have been placed
                GameObject.Find("Parameters").GetComponent<ParametersScript>().CheckEnd();
                    // to delete at the end
                print("Correct Imprint");
            // hits another imprint
            } else
            {
                    // to delete at the end
                print("Wrong Imprint");
            }
        }
    }

// Interactable establishes interaction with the object based on the bool placed
    public void Interactable(){
        this.GetComponent<NearInteractionGrabbable>().enabled = placed;
        this.GetComponent<ObjectManipulator>().enabled = placed;
    }
}
