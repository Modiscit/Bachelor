using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionScript : MonoBehaviour
{
    public float getLength(){
        if (this.tag == "Parameters" || this.tag == "Collection"){
            return 0f;
        } else {
            return this.GetComponent<MeshFilter>().mesh.bounds.extents.x * this.transform.localScale.x * 2;
        }
    }

    public float getDepth(){
        if (this.tag == "Parameters" || this.tag == "Collection"){
            return 0f;
        } else {
            return this.GetComponent<MeshFilter>().mesh.bounds.extents.z * this.transform.localScale.z * 2;
        }
    }

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

    public float getGlobalLength(){
        return GameObject.Find("Parameters").GetComponent<DimensionScript>().getChildGlobalLength(this.transform);
    }

    public float getGlobalDepth(){
        return GameObject.Find("Parameters").GetComponent<DimensionScript>().getChildGlobalDepth(this.transform);
    }
}