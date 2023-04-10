using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRLScript : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 differencePosition = Vector3.zero;


    // Update is called once per frame
    void Update()
    {
        setOrientation(cameraTransform);
        setPosition(differencePosition);
    }

    public void setRadiusAndPositionFieldAndColor(int angle, float distance, float radius, Material color){
        setRadius(radius);
        setPositionField(angle, distance);
        setColor(color);
    }

    private void setRadius(float radius){
        float scalemultiplier = radius*2/this.GetComponent<DimensionScript>().getChildGlobalDepth(this.transform);
        Vector3 newScale = this.transform.localScale;
        newScale.x *= scalemultiplier;
        newScale.z *= scalemultiplier;
        this.transform.localScale = newScale;
    }

    // Set the Vector3 position difference field of the PRL indicator
    // with z and x position dependent on the angle (0 being z positive) and the distance, y being as is
    private void setPositionField(int angle, float distance){
        this.differencePosition = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad)*distance, 
        0, 
        Mathf.Cos(angle * Mathf.Deg2Rad)*distance);
    }

    // set the color of the PRL indicator to the color given
    private void setColor(Material color){
        this.GetComponent<MeshRenderer>().material = color;
    }

    // set the orientation of the PRL indicator to face the transform (the camera)
    private void setOrientation(Transform cameraPosition){
        this.transform.forward = Vector3.up;
        // makes the forward (the positive z axis) face the camera
        this.transform.LookAt(cameraPosition);
        // rotate it to show the positive y axis
        transform.rotation *= Quaternion.FromToRotation(Vector3.up, Vector3.forward);
    }

    // set the position to be a difference to its parent center position
    private void setPosition(Vector3 difference){
        Bounds parentBounds= this.transform.parent.GetComponent<Renderer>().bounds;
        this.transform.position = parentBounds.center + new Vector3 (0,parentBounds.extents.y,0) + this.differencePosition;
    }
}
