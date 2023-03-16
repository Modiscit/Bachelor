using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametersScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // works with current hierarchy
    // checks all grandchildren of parameters to see if they can be interacted with
    // if they all can't call ApplyEnd()
    public void CheckEnd(){
        bool end = true;
        foreach (Transform child in transform){
            foreach (Transform greatchild in child){
                if (greatchild.tag == "Object"){
                    if (greatchild.GetComponent<ObjectScript>().can_interact){
                        end = false;
                        break;
                    }
                }
            }
        }
        if (end){
            ApplyEnd();
        }
    }

    void ApplyEnd(){
        print("You've successfully placed all objects");
    }
}
