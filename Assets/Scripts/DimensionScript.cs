using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionScript : MonoBehaviour
{
    // Get local length of the mesh, based on its bounds
    public float getLength(){
        if (this.tag == "Parameters" || this.tag == "Collection"){
            return 0f;
        } else {
            return this.GetComponent<MeshFilter>().mesh.bounds.extents.x * this.transform.localScale.x * 2;
        }
    }

    // Get local depth of the mesh, based on its bounds
    public float getDepth(){
        if (this.tag == "Parameters" || this.tag == "Collection"){
            return 0f;
        } else {
            return this.GetComponent<MeshFilter>().mesh.bounds.extents.z * this.transform.localScale.z * 2;
        }
    }

    // Get local height of the mesh, based on its bounds
    public float getHeight(){
        if (this.tag == "Parameters" || this.tag == "Collection"){
            return 0f;
        } else {
            return this.GetComponent<MeshFilter>().mesh.bounds.extents.y * this.transform.localScale.y * 2;
        }
    }

    // Get global length of the mesh belonging to the child Transform, based on its bounds
    public float getChildGlobalLength(Transform thing){
        float firstScaleX = this.transform.localScale.x;
        foreach (Transform child in transform){
            if (thing == child){
                return firstScaleX * child.GetComponent<DimensionScript>().getLength();
            } else { 
                foreach (Transform grandchild in child){
                    if (thing == grandchild){
                        return firstScaleX * child.localScale.x * grandchild.GetComponent<DimensionScript>().getLength();
                    } else {
                        foreach (Transform greatgrandchild in grandchild){
                            if (thing == greatgrandchild){
                                return firstScaleX * child.localScale.x * grandchild.localScale.x * greatgrandchild.GetComponent<DimensionScript>().getLength();
                            }
                        }
                    }
                }
            }
        }
        return firstScaleX * this.getLength();
    }

    // Get global depth of the mesh belonging to the child Transform, based on its bounds
    public float getChildGlobalDepth(Transform thing){
        float firstScaleX = this.transform.localScale.x;
        foreach (Transform child in transform){
            if (thing == child){
                return firstScaleX * child.GetComponent<DimensionScript>().getDepth();
            } else { 
                foreach (Transform grandchild in child){
                    if (thing == grandchild){
                        return firstScaleX * child.localScale.x * grandchild.GetComponent<DimensionScript>().getDepth();
                    } else {
                        foreach (Transform greatgrandchild in grandchild){
                            if (thing == greatgrandchild){
                                return firstScaleX * child.localScale.x * grandchild.localScale.x * greatgrandchild.GetComponent<DimensionScript>().getDepth();
                            }
                        }
                    }
                }
            }
        }
        return firstScaleX * this.getDepth();
    }

    // Get global height of the mesh belonging to the child Transform, based on its bounds
    public float getChildGlobalHeight(Transform thing){
        float firstScaleY = this.transform.localScale.y;
        foreach (Transform child in transform){
            if (thing == child){
                return firstScaleY * child.GetComponent<DimensionScript>().getHeight();
            } else { 
                foreach (Transform grandchild in child){
                    if (thing == grandchild){
                        return firstScaleY * child.localScale.y * grandchild.GetComponent<DimensionScript>().getHeight();
                    } else {
                        foreach (Transform greatgrandchild in grandchild){
                            if (thing == greatgrandchild){
                                return firstScaleY * child.localScale.y * grandchild.localScale.y * greatgrandchild.GetComponent<DimensionScript>().getHeight();
                            }
                        }
                    }
                }
            }
        }
        return firstScaleY * this.getHeight();
    }

    // Get global length of the mesh by calling the root of the hierarchy of objects, parameters, on itself
    public float getGlobalLength(){
        return GameObject.Find("Parameters").GetComponent<DimensionScript>().getChildGlobalLength(this.transform);
    }

    // Get global depth of the mesh by calling the root of the hierarchy of objects, parameters, on itself
    public float getGlobalDepth(){
        return GameObject.Find("Parameters").GetComponent<DimensionScript>().getChildGlobalDepth(this.transform);
    }

    // Get global height of the mesh by calling the root of the hierarchy of objects, parameters, on itself
    public float getGlobalHeight(){
        return GameObject.Find("Parameters").GetComponent<DimensionScript>().getChildGlobalHeight(this.transform);
    }
}
