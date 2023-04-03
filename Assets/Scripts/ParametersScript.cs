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
        if (mode == "blackonwhite" || mode == "whiteonblack"){
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
        applyObjects(this.objectsList, this.objectsNumberList);
        applyColors(this.colorsList, this.colorMode);
        applyPRL(this.PRL,this.PRL_angle, this.PRL_distance, this.PRL_radius, this.PRL_color);
        applyLays();
    }

    // this method is private because it would change the size of the PRL otherwise
    private void applyScale(float number){
        // get position relative to the HeightCalibrationMenu
        Vector3 previousPos = this.transform.position;
        GameObject HeightCalibrationMenu = GameObject.Find("HeightCalibrationMenu");
        Vector3 MenuEverythingDifference = HeightCalibrationMenu.transform.position - previousPos;
        print("position y of everything is : " + previousPos);
        // get slate and an imprint
        GameObject Slate = GameObject.Find("Slate");
        GameObject Imprint = GameObject.FindGameObjectWithTag("Imprint");
        // get distance y imprints to slate
        float tempGapY = Imprint.transform.position.y - Slate.transform.position.y;
        // change the scale of everything in regards to the actual scale
        this.transform.localScale = number * this.transform.localScale;
        // reset the distance y, imprints to slate, to what it was based on the difference
        float gapYDifference =  Imprint.transform.position.y - Slate.transform.position.y - tempGapY;
        Vector3 actualSlatePosition = Slate.transform.position;
        actualSlatePosition.y += gapYDifference;
        Slate.transform.position = actualSlatePosition;
        // reset position of everything in space to HeightCalibrationMenu, to what it was based on the scaled difference
        this.transform.position = HeightCalibrationMenu.transform.position - new Vector3 (number*MenuEverythingDifference.x,MenuEverythingDifference.y,number*MenuEverythingDifference.z);
    }

    // for this method to work, the imprints need to have the same name as the objects + Imprint.
    // can be called only once if there was a 0 in the list.
    private void applyObjects(List<Transform> piecesList, List<int> piecesNumbersList){
        GameObject ObjectsCollection = GameObject.Find("ObjectsCollection");
        GameObject ImprintsCollection = GameObject.Find("ImprintsCollection");
        int tempIndex = 0;
        foreach (int number in piecesNumbersList){
            GameObject piece = piecesList[tempIndex++].gameObject;
            GameObject pieceImprint = GameObject.Find(piece.name + "Imprint");
            if (number < 1){
                // necessary else the parent still counts as having that child, even though it is dead
                piece.transform.parent = null;
                pieceImprint.transform.parent = null;
                Destroy(piece);
                Destroy(pieceImprint);
            } else if (number > 1){
                for (int i = 1; i < number; i++){
                    GameObject pieceClone = Instantiate(piece, piece.transform.position, piece.transform.rotation);
                    GameObject pieceImprintClone = Instantiate(pieceImprint, pieceImprint.transform.position, pieceImprint.transform.rotation);
                    pieceClone.transform.parent = piece.transform.parent;
                    pieceImprintClone.transform.parent = pieceImprint.transform.parent;
                    pieceClone.transform.localScale = piece.transform.localScale;
                    pieceImprintClone.transform.localScale = pieceImprint.transform.localScale;
                }
            }
        }
        print("the objecCollection has " + ObjectsCollection.transform.childCount + " children from the parameters");
    }

    // can't be called twice as the imprints and the objects pairs don't have the same indices after intialization
    // in case this methods is called multiple times, uncomment the changing of the slate color
    private void applyColors(List<Material> materials, string coloring){
        GameObject ObjectsCollection = GameObject.Find("ObjectsCollection");
        GameObject ImprintsCollection = GameObject.Find("ImprintsCollection");
        int numberOfChildren = ImprintsCollection.transform.childCount;
        int currentImprintNumber = 0;
        // just to have a color in case coloring is neither of the three values
        Material color = materials[0];
        if (coloring.Equals("blackonwhite")){
            color = (Material)Resources.Load("Black", typeof(Material));
            //GameObject.Find("Slate").GetComponent<MeshRenderer>().material = (Material)Resources.Load("WhiteSlate", typeof(Material));
        } else if (coloring.Equals("whiteonblack")){
            color = (Material)Resources.Load("White", typeof(Material));
            GameObject.Find("Slate").GetComponent<MeshRenderer>().material = (Material)Resources.Load("BlackSlate", typeof(Material));
        } //else {
            //GameObject.Find("Slate").GetComponent<MeshRenderer>().material = (Material)Resources.Load("WhiteSlate", typeof(Material));
        //}
        foreach (Transform objectChild in ObjectsCollection.transform){
            if (coloring.Equals("normal")){
                color = materials[Random.Range(0, materials.Count)];
            }
            objectChild.GetComponent<MeshRenderer>().material = color;
            ImprintsCollection.transform.GetChild(currentImprintNumber++).GetComponent<MeshRenderer>().material = color;
        }

    }

    // create PRL circles children of the pieces. PRL prefab has to be in a Resources folder and called PRL.
    private void applyPRL(bool enabled, int angle, float distance, float radius, Material color){
        if (enabled){
            Transform objectsCollection = GameObject.Find("ObjectsCollection").transform;
            GameObject PRLprefab = (GameObject)Resources.Load("PRL", typeof(GameObject));
            // for each child object of objects collection, create the PRL prefab and assign the object as its parent
            // call a function to set its values
            foreach (Transform objectChild in objectsCollection){
                GameObject PRL = Instantiate(PRLprefab, objectChild);
                PRL.GetComponent<PRLScript>().setRadiusAndPositionFieldAndColor(angle,distance,radius,color);
            }
        }
    }

    private void applyLays(){
        Transform slate = GameObject.FindGameObjectWithTag("Slate").transform;
        GameObject.Find("ObjectsCollection").GetComponent<ObjectsCollectionScript>().Lay(slate);
        GameObject.Find("ImprintsCollection").GetComponent<ImprintsCollectionScript>().Lay(slate);
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
