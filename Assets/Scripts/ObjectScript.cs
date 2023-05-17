using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    // this boolean represents whether the object can be interacted with or not
    public bool can_interact = false;
    // this is the number of errors made regardless of type
    public int number_of_errors = 0;
    // this is the number of errors when the color is different
    public int number_of_color_errors = 0;
    // this is the number of errors when the shape is different
    public int number_of_shape_errors = 0;
    // this is to know whether the last imprint that was collided with is the correct one
    // it is used to now whether to save the direction of errors or not
    public bool last_collision_correct = false;
    // this saves the last imprint that was collided with
    public GameObject last_collision_imprint = null;
    // this is to save when the object was first grabbed
    public float first_time = 0f;
    // this is to save when the object was last released
    public float last_time = 0f;

// This is used in the attempt to make the slate solid
/*     public Vector3 PositionOfContact;
    public string collisionDim; */
 
    // This method needs a rigidbody to function and boxcolliders
    // It is called when a the associated object's rigidbody collides with another object
    // It is called with the information on the collision
    private void OnCollisionEnter(Collision collision)
    {
        bool isOfColor = false;
        bool isOfShape = false;
        // checks if the collision was in movement mode and with an imprint different from the last one
        if(can_interact && collision.transform.tag == "Imprint" && !collision.gameObject.Equals(last_collision_imprint)) {
            // record the imprint it collided with
            last_collision_imprint = collision.gameObject;
            // checks if it is with the same shape, checks the name of the model (defined in the 3D modeling tool)
            if (last_collision_imprint.transform.GetComponent<MeshFilter>().mesh.name.Equals(this.transform.GetComponent<MeshFilter>().mesh.name)) {
                isOfShape = true;
            } else {
                number_of_shape_errors++;
            }
            // checks if it is with the same color
            if (last_collision_imprint.transform.GetComponent<MeshRenderer>().material.name == this.transform.GetComponent<MeshRenderer>().material.name){
                isOfColor = true;
            } else {
                number_of_color_errors++;
            }
            // if it is with a correct imprint
            if (isOfColor && isOfShape){
                last_collision_correct = true;
            // hits something else
            } else
            {
                last_collision_correct = false;
                number_of_errors++;
            }
        }
            // This is used in the attempt to make the slate solid
/*         if (collision.transform.tag == "Slate"){
            PositionOfContact = this.transform.position;
            collisionDim = collisionDimension(collision.GetContact(0).point, GameObject.Find("Slate"));
        }  */
    }

// this was made so the pieces would not go through the slate
// the code is functional BUT it depends on collision detection
// collision detection is detected too early in the y position, thus the object cannot be put close to the imprint
// if collision detection is fixed, this code can be used

/*     private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.transform.tag == "Slate"){
            Vector3 pos = this.transform.position;
            switch (collisionDim){
                case "X":
                    pos.x = PositionOfContact.x;
                    break;
                case "Y":
                    pos.y = PositionOfContact.y;
                    break;
                default:
                    pos.z = PositionOfContact.z;
                    break;
            }
            this.transform.position = pos;
        }
    }
 */
/*     // I tried with globalDepth and globalDepth, but it worked less well than this
    public string collisionDimension(Vector3 point, GameObject obj){
        Bounds b = obj.GetComponent<MeshFilter>().mesh.bounds;
        Vector3 difference = point - b.center;
        if (Mathf.Abs(difference.x) >= b.extents.x/2){
            return "X";
        } else if (Mathf.Abs(difference.y) >= b.extents.y/2){
            return "Y";
        } else {
            return "Z";
        }
    } */

    // This becomes the object in use and if an object was being used, it can't be interacted with anymore.
    // It also changes the color of the outline to Grab Material
    public void OnGrab(){
        setFirstGrabbedTime();
        GameObject wasInteracting = this.transform.parent.GetComponent<ObjectsCollectionScript>().isInteractingWith;
        if (wasInteracting == null){
            this.transform.parent.GetComponent<ObjectsCollectionScript>().isInteractingWith = this.gameObject;
        } else if (wasInteracting != this.gameObject){
            wasInteracting.GetComponent<ObjectScript>().Terminate();
            this.transform.parent.GetComponent<ObjectsCollectionScript>().isInteractingWith = this.gameObject;
        }
        setOutlineColor("Grab");
    }

    // Set the last released time and changed back the color to Hover Material
    public void OnRelease(){
        setLastReleasedTime();
        setOutlineColor("Release");
    }

    // Activates the outline of the object to hint that it can be grabbed
    // The outline script shouldn't be activated right from the start, else there is an issue
    public void OnHoverEnter(){
        setOutlineColor("HoverEnter");
    }

    // Deactivate outline to hint that it can't be grabbed
    public void OnHoverExit(){
        setOutlineColor("HoverExit");
    }

    // Interactable establishes interaction with the object based on the bool can_interact
    public void Interactable(){
        this.GetComponent<NearInteractionGrabbable>().enabled = can_interact;
        this.GetComponent<ObjectManipulator>().enabled = can_interact;
    }

    // Deactivate the object
    public void Terminate(){
        can_interact = false;
        Interactable();
    }

    // This create an ObjectRecords with information about the object and returns it
    public ObjectRecords GetRecords(){
        ObjectRecords record = new ObjectRecords();
        record.type = this.GetComponent<MeshFilter>().mesh.name;
        record.color = this.GetComponent<MeshRenderer>().material.name;
        record.number_of_errors = number_of_errors;
        record.number_of_color_errors = number_of_color_errors;
        record.number_of_shape_errors = number_of_shape_errors;
        // get duration in seconds from last released and first grabbed
        record.time_interacting = last_time - first_time;
        record.direction_of_error = GetDirectionOfError();
        record.rotation_error = GetRotationError();
        return record;
    }

    // This returns the difference between the object to its correct imprint if they were last in interaction
    public DirectionRecords GetDirectionOfError(){
        DirectionRecords record = new DirectionRecords();
        // if no collision was detected or the last collision was not the correct imprint set to 0,0,0
        if (last_collision_imprint == null || !last_collision_correct){
            record.x = 0f;
            record.y = 0f;
            record.z = 0f;
        // else set to the difference of centers vectors
        } else {
            Vector3 ObjectCenter = this.GetComponent<Renderer>().bounds.center;
            Vector3 LastCollisionImprintCenter = last_collision_imprint.GetComponent<Renderer>().bounds.center;
            record.x = ObjectCenter.x - LastCollisionImprintCenter.x;
            record.y = ObjectCenter.y - LastCollisionImprintCenter.y;
            record.z = ObjectCenter.z - LastCollisionImprintCenter.z;
        }
        return record;
    }

    // This returns the difference of Vector3 rotation between the imprint and the piece, if it was correct
    public RotationRecords GetRotationError(){
        RotationRecords record = new RotationRecords();
        // if no collision was detected or the last collision was not the correct imprint set to 0,0,0
        if (last_collision_imprint == null || !last_collision_correct){
            record.x_angle = 0f;
            record.y_angle = 0f;
            record.z_angle = 0f;
        // else set to the difference of rotations
        } else {
            Vector3 ObjectRotation = this.transform.rotation.eulerAngles;
            Vector3 LastCollisionImprintRotation = last_collision_imprint.transform.rotation.eulerAngles;
            record.x_angle = ObjectRotation.x - LastCollisionImprintRotation.x;
            record.y_angle = ObjectRotation.y - LastCollisionImprintRotation.y;
            record.z_angle = ObjectRotation.z - LastCollisionImprintRotation.z;
        }
        return record;
    }

    // the field first_time is instantiated, this is called when it is grabbed for the first time
    public void setFirstGrabbedTime(){
        if (first_time == 0){
            first_time = Time.time;
        }
    }

    // the field last_time is set to the current time, this is called everytime the object is released
    public void setLastReleasedTime(){
        last_time = Time.time;
    }

    // activate, deactivate, change colors of the outline
    // for some reasong any changes set the color back, so we reset it after the changes
    // default is HoverExit, which disables the outline
    public void setOutlineColor(string mode="HoverExit"){
        Material color = this.GetComponent<MeshRenderer>().material;
        switch (mode){
            case "HoverEnter":
                this.GetComponent<MeshOutline>().enabled = true;
                break;
            case "Grab":
                this.GetComponent<MeshOutline>().OutlineMaterial = (Material)Resources.Load("GrabOutline", typeof(Material));
                break;
            case "Release":
                this.GetComponent<MeshOutline>().OutlineMaterial = (Material)Resources.Load("HoverOutline", typeof(Material));
                break;
            default:
                this.GetComponent<MeshOutline>().enabled = false;
                break;
        }
        this.GetComponent<MeshRenderer>().material = color;
    }
}
