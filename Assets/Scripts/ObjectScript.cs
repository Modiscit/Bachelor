using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{

    public GameObject Imprint;
    public bool can_interact;

    // Start is called before the first frame update
    void Start()
    {
        // should be false when I can interact with button in Unity, false for Hololens
        can_interact = true;
        Interactable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // checks if the collision is with an imprint
        if(can_interact && collision.transform.tag == "Imprint")
        {   // hits the right imprint
            if(collision.transform.name == Imprint.name)
            {
                // stops interaction
                can_interact = false;
                Interactable();
                // stops movement
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
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
        this.GetComponent<NearInteractionGrabbable>().enabled = can_interact;
        this.GetComponent<ObjectManipulator>().enabled = can_interact;
    }

}
