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

/*     private void OnCollisionEnter(Collision collision)
    {
        bool isOfColor = false;
        bool isOfShape = false;
        // checks if the collision was in movement mode and with an imprint
        // TO ADD on release
        if(can_interact && collision.transform.tag == "Imprint") {
            // checks if it is with the same shape, checks the name of the model (defined in the 3D modeling tool)
            if (collision.transform.GetComponent<MeshFilter>().mesh.name.Equals(this.transform.GetComponent<MeshFilter>().mesh.name)) {
                print("it has the same shape");
                isOfShape = true;
            }
            // checks if it is with the same color
            if (collision.transform.GetComponent<MeshRenderer>().material.name == this.transform.GetComponent<MeshRenderer>().material.name){
                print("it has the same color");
                isOfColor = true;
            }
            // if it is with a correct imprint
            if (isOfColor && isOfShape){
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
            // hits something else
            } else
            {
                    // to delete at the end
                print("Error");
            }
        }
    } */

    // This becomes the object in use and if an object was being used, it can't be interacted with anymore.
    public void OnGrab(){
        GameObject wasInteracting = this.transform.parent.GetComponent<ObjectsCollectionScript>().isInteractingWith;
        if (wasInteracting == null){
            this.transform.parent.GetComponent<ObjectsCollectionScript>().isInteractingWith = this.gameObject;
        } else if (wasInteracting != this.gameObject){
            wasInteracting.GetComponent<ObjectScript>().Terminate();
            this.transform.parent.GetComponent<ObjectsCollectionScript>().isInteractingWith = this.gameObject;
        }
    }

// Interactable establishes interaction with the object based on the bool placed
    public void Interactable(){
        this.GetComponent<NearInteractionGrabbable>().enabled = can_interact;
        this.GetComponent<ObjectManipulator>().enabled = can_interact;
    }

    public void Terminate(){
        can_interact = false;
        Interactable();
    }

}
