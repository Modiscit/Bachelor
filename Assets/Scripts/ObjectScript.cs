using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public bool can_interact;
    public int number_of_errors = 0;
    public int number_of_color_errors = 0;
    public int number_of_shape_errors = 0;
    public bool last_collision_correct = false;
    public GameObject last_collision_imprint = null;
    public float first_time = 0f;
    public float last_time = 0f;

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
        bool isOfColor = false;
        bool isOfShape = false;
        // checks if the collision was in movement mode and with an imprint different from the last one
        if(can_interact && collision.transform.tag == "Imprint" && collision.gameObject != last_collision_imprint) {
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
                // stops movement, shouldn't need to
                // this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                // this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                // checks if all objects have been placed
                // GameObject.Find("Parameters").GetComponent<ParametersScript>().CheckEnd();
                // to delete at the end
                // print("Correct Imprint");
            // hits something else
            } else
            {
                last_collision_correct = false;
                number_of_errors++;
            }
        }
    }

    // This becomes the object in use and if an object was being used, it can't be interacted with anymore.
    public void OnGrab(){
        setFirstGrabbedTime();
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

    public void setFirstGrabbedTime(){
        if (first_time == 0){
            first_time = Time.time;
        }
    }

    public void setLastReleasedTime(){
        last_time = Time.time;
    }
}
