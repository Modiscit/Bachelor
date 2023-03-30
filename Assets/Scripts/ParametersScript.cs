using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametersScript : MonoBehaviour
{
    // the JSON file loaded in Unity
    public TextAsset jsonFile;
    // the parameters from the JSON
    public string userName;
    public bool anonimity;
    public List<Transform> objectsList;
    public List<int> objectsNumberList;
    public float scale;
    public List<Material> colorsList;
    public string colorMode;
    public bool PRL;
    public int PRL_angle;
    public float PRL_distance;
    public float PRL_radius;
    public Material PRL_color;
    // Start is called before the first frame update
    void Start()
    {
        Parameters parametersInJson = readJson(jsonFile);
        ParametersToUnityParameters(parametersInJson);
        ApplyUnityParameters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Parameters readJson(TextAsset json){
        Parameters parametersInJson = JsonUtility.FromJson<Parameters>(json.text);
        print(parametersInJson.objects[0].name);
        return parametersInJson;
    }

    private void ParametersToUnityParameters(Parameters parametersObject){
        setName(parametersObject.name);
        setAnonimity(parametersObject.anonimity);
        setObjects(parametersObject.objects);
        setScale(parametersObject.scale);
        setColors(parametersObject.colors);
        setColorMode(parametersObject.colormode);
        setPRL(parametersObject.PRL,parametersObject.PRL_angle,
        parametersObject.PRL_distance,parametersObject.PRL_size,parametersObject.PRL_color);
    }

    // All setters
    // set the name of the user, default is "anonymous"
    public void setName(string text="anonymous"){
        this.userName = name;
    }

    // set the anonimity, default is false
    public void setAnonimity(bool choice=false){
        this.anonimity = choice;
    }

    // set the type and number of objects, default is 2 Square
    public void setObjects(Object[] piecesList){
        foreach (Object piece in piecesList){
            Transform temp = GameObject.Find(piece.name).transform;
            if (temp != null){
                this.objectsList.Add(temp);
                this.objectsNumberList.Add(piece.number);
            }
        }
        if (this.objectsList.Count == 0){
            this.objectsList.Add(GameObject.Find("Square").transform);
            this.objectsNumberList.Add(2);
        }
    }

    // set scale to the number in between bounds, default is 1
    public void setScale(float number=1f){
        float minScale = 0.1f;
        float maxScale = 1f;
        this.scale = Mathf.Max(Mathf.Min(maxScale,number),minScale);
    }

    // add to colorsList the materials in the folder Resources (compulsory name) corresponding to the names in the list
    // default is Blue
    public void setColors(string[] colorsNameList){
        this.colorsList = new List<Material>();
        foreach (string colorName in colorsNameList){
            Material tempMaterial = (Material)Resources.Load(colorName, typeof(Material));
            if (tempMaterial != null){
                this.colorsList.Add(tempMaterial);
            }
        }
        if (this.colorsList.Count == 0){
            this.colorsList.Add((Material)Resources.Load("Blue", typeof(Material)));
        }
    }

    // set colodMode to one of three values, default is "normal"
    public void setColorMode(string mode="normal"){
        if (mode == "blackandwhite" || mode == "whiteandblack"){
            this.colorMode = mode;
        } else {
            this.colorMode = "normal";
        }
    }

    // set values for PRL fields
    // angle is between 0 and 359
    // radius is at most the same as the distance
    // the default material is Red
    // default is disabled
    public void setPRL(bool active=false, int angle=45, float distance=0.1f, float size=0.05f, string color="Red"){
        this.PRL = active;
        this.PRL_angle = angle % 360;
        this.PRL_distance = distance;
        this.PRL_radius = size < distance ? size : distance;
        Material tempMaterial = (Material)Resources.Load(color, typeof(Material));
        if (tempMaterial == null){
            tempMaterial = (Material)Resources.Load("Red", typeof(Material));
        }
        this.PRL_color = tempMaterial;
    }

    // All apply
    public void ApplyUnityParameters(){
        applyScale(this.scale);
        //applyObjects(this.objectsList, this.objectsNumberList);
        //applyColors(this.colorsList, this.colorMode);
        //applyPRL(this.PRL,this.PRL_angle, this.PRL_distance, this.PRL_radius, this.PRL_color);
    }

    // this method is private because it would change the size of the PRL otherwise
    private void applyScale(float number){
        // get y position of slate in space
        // maybe get distance y imprints to slate
        this.transform.localScale = number * this.transform.localScale;
        // maybe reset the distance y imprints to slate to what it was, check if necessary with min and max scale
        // set y position of slate in space to where it was
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
