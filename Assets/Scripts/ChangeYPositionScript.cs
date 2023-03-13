using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeYPositionScript : MonoBehaviour
{
    public void Lower(){
        this.transform.position += new Vector3(0,-0.01f,0);
    }

    public void Higher(){
        this.transform.position += new Vector3(0,0.01f,0);
    }

    public void Validate(){
        foreach (Transform child in transform){
            if (child.tag == "Object"){
                child.GetComponent<ObjectScript>().placed = true;
                child.GetComponent<ObjectScript>().Interactable();
            }
        }
    }
}