using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeYPositionScript : MonoBehaviour
{
    // lower the slate
    public void Lower(){
        this.transform.position += new Vector3(0,-0.01f,0);
    }

    // elevates the slate
    public void Higher(){
        this.transform.position += new Vector3(0,0.01f,0);
    }

    // calls every grandchildren and if they have the tag object, they are interactable once more
    public void Validate(){
        this.GetComponent<ParametersScript>().start = Time.time;
        foreach (Transform child in this.transform){
            foreach (Transform greatchild in child){
                if (greatchild.tag == "Object"){
                    greatchild.GetComponent<ObjectScript>().can_interact = true;
                    greatchild.GetComponent<ObjectScript>().Interactable();
                }
            }
        }
        // to delete
        print("Interactable once more");
    }
}