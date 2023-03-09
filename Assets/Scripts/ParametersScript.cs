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

    public void CheckEnd(){
        bool end = true;
        foreach (Transform child in transform){
            if (child.tag == "Object"){
                if (!child.GetComponent<ObjectScript>().placed){
                    end = false;
                    break;
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
