using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRLScript : MonoBehaviour
{
    // needs to know where the eyes of the user are
    public Transform cameraTransform;
    // difference in the space between the PRL origin and the piece origin
    public Vector3 differencePosition = Vector3.zero;


    // Update is called once per frame
    // Update the orientation and the position of the PRL dot to compensate for movement of the piece and the user
    void Update()
    {
        setOrientation(cameraTransform);
        setPosition(differencePosition);
    }

    // set the difference in position with the piece based on the radius, the angle and the distance given
    // set the color of the PRL dot
    public void setRadiusAndPositionFieldAndColor(int angle, float distance, float radius, Material color){
        setRadius(radius);
        setPositionField(angle, distance);
        setColor(color);
    }

    // set the size of the PRL dot regardless of the scale
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

    // set the position to be a difference to its parent center position and make it slightly higher so that it doesn't render in the same space
    // (there is an argument to make it a radius higher, as if we look at the piece, the dot turns to us, 
    // at most it will turn with half of the dot below that point, thus hiding half the point in that configuration)
    private void setPosition(Vector3 difference){
        Bounds parentBounds= this.transform.parent.GetComponent<Renderer>().bounds;
        this.transform.position = parentBounds.center + new Vector3 (0,parentBounds.extents.y + 0.000001f) + this.differencePosition;
    }
}
