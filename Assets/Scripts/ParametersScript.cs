using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametersScript : MonoBehaviour
{
    // the JSON file loaded in Unity
    public TextAsset jsonFile;
    // Start is called before the first frame update
    void Start()
    {
        Parameters parametersInJson = readJson(jsonFile);
        //ParametersToUnityParameters(parametersInJson);
        //ApplyUnityParameters(allparameters);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Parameters readJson(TextAsset json){
        Parameters parametersInJson = JsonUtility.FromJson<Parameters>(json.text);
        print(parametersInJson.objects[0].name);
        return parametersInJson;
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
