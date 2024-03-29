using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ParametersScript : MonoBehaviour
{
    // the JSON file loaded in Unity
    public TextAsset jsonFile;
    // the parameters from the JSON
    public string user_name;
    public bool anonimity;
    public List<Transform> objectsList;
    public List<int> objectsNumberList;
    public float scale;
    public string scalemode;
    public bool limit_to_field_of_view;
    public string rotationmode;
    public List<Material> colorsList;
    public string colorMode;
    public bool PRL;
    public int PRL_angle;
    public float PRL_distance;
    public float PRL_radius;
    public Material PRL_color;

    // the fields to record the start and end time of the task
    public float start = 0f;
    public float end = 0f;

    // Start is called before the first frame update
    // read the associated JSON file
    // set the fields
    // apply the parameters
    void Start()
    {
        Parameters parametersInJson = readJson(jsonFile);
        ParametersToUnityParameters(parametersInJson);
        ApplyUnityParameters();
    }

    // Creates a Parameters object from the associated JSON file
    private Parameters readJson(TextAsset json){
        Parameters parametersInJson = JsonUtility.FromJson<Parameters>(json.text);
        return parametersInJson;
    }

    // Set the fields to the Parameters object
    // This is a conversion of basic types to unity types
    private void ParametersToUnityParameters(Parameters parametersObject){
        setName(parametersObject.user_name);
        setAnonimity(parametersObject.anonimity);
        setObjects(parametersObject.objects);
        setScale(parametersObject.scale, parametersObject.limit_to_field_of_view);
        setScaleMode(parametersObject.scalemode);
        setRotationMode(parametersObject.rotationmode);
        setColors(parametersObject.colors);
        setColorMode(parametersObject.colormode);
        setPRL(parametersObject.PRL,parametersObject.PRL_angle,
        parametersObject.PRL_distance,parametersObject.PRL_size,parametersObject.PRL_color);
    }

    // All setters
    // set the name of the user, default is "anonymous"
    public void setName(string text="anonymous"){
        this.user_name = text;
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
    // can also be bounded by the field of view
    public void setScale(float number=1f, bool limit=true){
        float minScale = 0.1f;
        float maxScale = 1f;
        // empirical value that maximize the field of view
        float maxFieldOfView = 0.3f;
        if (limit){
            this.scale = Mathf.Min(maxFieldOfView,number);
        } else {
            this.scale = Mathf.Max(Mathf.Min(maxScale,number),minScale);
        }
    }

    // set scalemode to either "normal" or "imprintsonly"
    // default is "normal"
    public void setScaleMode(string mode="normal"){
        if (mode.Equals("imprintsonly")){
            this.scalemode = mode;
        } else {
            this.scalemode = "normal";
        }
    }

    // set rotationmode to either "free", "xzlocked" or "locked"
    // default is "free"
    public void setRotationMode(string mode="free"){
        if (mode.Equals("xzlocked") || mode.Equals("locked")){
            this.rotationmode = mode;
        } else {
            this.rotationmode = "free";
        }
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
    // the default material is Red
    // size can't be smaller than 0.1
    // default is disabled
    public void setPRL(bool active=false, int angle=45, float distance=0.1f, float size=0.05f, string color="Red"){
        this.PRL = active;
        this.PRL_angle = angle % 360;
        this.PRL_distance = distance;
        this.PRL_radius = size < 0.1f ? 0.1f : size;
        Material tempMaterial = (Material)Resources.Load(color, typeof(Material));
        if (tempMaterial == null){
            tempMaterial = (Material)Resources.Load("Red", typeof(Material));
        }
        this.PRL_color = tempMaterial;
    }

    // All apply
    // The order matter as it modifies the prefabs, then copy them, then randomize colors, then Lays them
    public void ApplyUnityParameters(){
        applyScale(this.scale, this.scalemode);
        applyRotationMode(this.rotationmode);
        applyObjects(this.objectsList, this.objectsNumberList);
        applyColors(this.colorsList, this.colorMode);
        applyPRL(this.PRL,this.PRL_angle, this.PRL_distance, this.PRL_radius, this.PRL_color);
        applyLays();
    }

    // this method is private because it would change the size of the PRL otherwise
    // default is no change in scale and everything scale normally
    private void applyScale(float number=1f, string mode="normal"){
        GameObject Slate = GameObject.Find("Slate");
        // if it is imprintsonly, only the imprints will be scaled
        // doesn't take into account field_of_view
        if (mode.Equals("imprintsonly")){
            GameObject ImprintsCollections = GameObject.Find("ImprintsCollection");
            foreach (Transform imprintChild in ImprintsCollections.transform){
                float tempGapY = imprintChild.position.y - Slate.transform.position.y;
                Vector3 newLocalScale = imprintChild.localScale;
                newLocalScale.x *= number;
                newLocalScale.z *= number;
                imprintChild.localScale = newLocalScale;
                Vector3 tempPosition = imprintChild.position;
                tempPosition.y = Slate.transform.position.y + tempGapY;
                imprintChild.position = tempPosition;
            }
        } else {
            // get position relative to the HeightCalibrationMenu
            Vector3 previousPos = this.transform.position;
            GameObject HeightCalibrationMenu = GameObject.Find("HeightCalibrationMenu");
            Vector3 MenuEverythingDifference = HeightCalibrationMenu.transform.position - previousPos;
            // get termination menu position relative to the slate end
            GameObject TerminationMenu = GameObject.Find("TerminationMenu");
            Vector3 TerminationSlateEndDifference = TerminationMenu.transform.position - (Slate.transform.position + new Vector3 (Slate.GetComponent<DimensionScript>().getGlobalLength()/2,0f,0f));
            // get an imprint
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
            // reset position x of the TerminationMenu
            Vector3 TerminationMenuNewPos = TerminationMenu.transform.position;
            TerminationMenuNewPos.x = Slate.transform.position.x + Slate.GetComponent<DimensionScript>().getGlobalLength()/2 + TerminationSlateEndDifference.x;
            TerminationMenu.transform.position = TerminationMenuNewPos;
        }
    }

    // lock the rotation of the objects based on mode
    // called only once, if multiple calls, add a third else to unlock the rotation and check that nothing has been locked differently before
    private void applyRotationMode(string mode){
        // enable rotation only on the Y axis
        if (mode.Equals("xzlocked")){
            foreach (Transform childObject in this.objectsList){
                RotationAxisConstraint XAxisRotationContraint = childObject.gameObject.AddComponent<RotationAxisConstraint>();
                RotationAxisConstraint ZAxisRotationContraint = childObject.gameObject.AddComponent<RotationAxisConstraint>();
                XAxisRotationContraint.ConstraintOnRotation = AxisFlags.XAxis;
                ZAxisRotationContraint.ConstraintOnRotation = AxisFlags.ZAxis;
            }
        // disable all rotations
        } else if (mode.Equals("locked")){
            foreach (Transform childObject in this.objectsList){
                FixedRotationToWorldConstraint AxisRotationContraint = childObject.gameObject.AddComponent<FixedRotationToWorldConstraint>();
            }
        }
    }

    // for this method to work, the imprints need to have the same name as the objects + "Imprint".
    // can be called only once if there was a 0 in the list as the object won't be able to be created without an original.
    private void applyObjects(List<Transform> piecesList, List<int> piecesNumbersList){
        GameObject ObjectsCollection = GameObject.Find("ObjectsCollection");
        GameObject ImprintsCollection = GameObject.Find("ImprintsCollection");
        int tempIndex = 0;
        foreach (int number in piecesNumbersList){
            GameObject piece = piecesList[tempIndex++].gameObject;
            GameObject pieceImprint = GameObject.Find(piece.name + "Imprint");
            if (number < 1){
                // necessary else the parent still counts as having that child, even though it is dead
                // it will cause issues with traversing children otherwise
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
    }

    // apply colors to the slate, objects and imprints based on the Material list and the coloring chosen
    // can't be called twice as the imprints and the objects pairs don't have the same indices after intialization
    // in case this method is called multiple times, uncomment the changing of the slate color
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
                color = materials[UnityEngine.Random.Range(0, materials.Count)];
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

    // Apply the randomized placement of imprints and pieces
    private void applyLays(){
        Transform slate = GameObject.FindGameObjectWithTag("Slate").transform;
        GameObject.Find("ObjectsCollection").GetComponent<ObjectsCollectionScript>().Lay(slate);
        GameObject.Find("ImprintsCollection").GetComponent<ImprintsCollectionScript>().Lay(slate);
    }

    // Stops all pieces from being interactable and record data
    public void Terminate(){
        GameObject ObjectsCollection = GameObject.Find("ObjectsCollection");
        foreach (Transform ObjectChild in ObjectsCollection.transform){
            ObjectChild.GetComponent<ObjectScript>().Terminate();
        }
        Record();
    }

    // Create a Records and transform it into a JSON file to be saved
    public void Record(){
        Records Recordfile = new Records();
        // Get time between validation of height and validation of task
        Recordfile.time_total = end - start;
        // Getting each objects records
        GameObject ObjectsCollection = GameObject.Find("ObjectsCollection");
        Recordfile.objects = new ObjectRecords[ObjectsCollection.transform.childCount];
        int tempIndex = 0;
        // int numOfPerfects = 0;
        // float totalDifPercentage = 0f;
        foreach (Transform objectChild in ObjectsCollection.transform){
            ObjectRecords tempObjRec = objectChild.GetComponent<ObjectScript>().GetRecords();
            Recordfile.objects[tempIndex++] = tempObjRec;
            // numOfPerfects += tempObjRec.number_of_errors > 0 ? 0 : 1;
            // totalDifPercentage += getPercentageOfError(objectChild, tempObjRec);
        }
        Recordfile.scale = scale;
        Recordfile.scalemode = scalemode;
        Recordfile.limit_to_field_of_view = limit_to_field_of_view;
        Recordfile.rotationmode = rotationmode;
        Recordfile.colors = getNameOfColors(colorsList);
        Recordfile.colormode = colorMode;
        Recordfile.PRL = PRL;
        Recordfile.PRL_color = PRL_color.name;
        Recordfile.PRL_size = PRL_radius;
        Recordfile.PRL_angle = PRL_angle;
        Recordfile.PRL_distance = PRL_distance;
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(Recordfile);
        // find a path to store the file, name the file depending on anonimity
        string saveFile = Application.persistentDataPath + "/" + getName() + ".json";
        // print("path where the JSON is : " + Application.persistentDataPath);
        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);

        // this was an attempt at a future score window, it was abandoned, as it was something discussed with supervisors, but not Laurie
        // the project is in codesign with the FSA, thus I could not add in new features without prior discussions
        // feedback to users would be done approprietly by the low vision specialist

        // print("time taken : " + Recordfile.time_total + "s");
        // print("number of pieces placed without errors" + numOfPerfects +"/" + Recordfile.objects.Length);
        // print("precision : " + totalDifPercentage*100/Recordfile.objects.Length + "%");
    }

    // should return a string like 23_04_11_15_26_name or hash based on anonimity
    public string getName(){
        string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_");
        string nameOfFile = date + user_name;
        if (anonimity){
            // return hash
            return nameOfFile.GetHashCode().ToString();
        }
        return nameOfFile;
    }

    // returns an array of colors names from a list of materials
    public string[] getNameOfColors(List<Material> colors){
        string[] colorsName = new string[colors.Count];
        for (int i = 0; i < colors.Count; i++){
            colorsName[i] = colors[i].name;
        }
        return colorsName;
    }

    // in the future should change to the area covered over the total area
    // height could also be changed as the piece center will be higher than the imprint center
/*     public float getPercentageOfError(Transform piece, ObjectRecords pieceRec){
        // calculate if the difference in x position is between 0 and the length of the piece
        // calculate the percent of that difference to the length and multiply the overall error percentage
        float tempPercent = 1;
        float tempLength = piece.GetComponent<DimensionScript>().getGlobalLength();
        float normalizedDifference = Math.Abs(pieceRec.direction_of_error.x) - tempLength;
        normalizedDifference = normalizedDifference < 0 ? 0 : normalizedDifference;
        tempPercent *= normalizedDifference / tempLength;

        // same for depth
        float tempDepth = piece.GetComponent<DimensionScript>().getGlobalDepth();
        normalizedDifference = Math.Abs(pieceRec.direction_of_error.z) - tempDepth;
        normalizedDifference = normalizedDifference < 0 ? 0 : normalizedDifference;
        tempPercent *= normalizedDifference / tempDepth;

        // same for height
        float tempHeight = piece.GetComponent<DimensionScript>().getGlobalHeight();
        normalizedDifference = Math.Abs(pieceRec.direction_of_error.y) - tempHeight;
        normalizedDifference = normalizedDifference < 0 ? 0 : normalizedDifference;
        tempPercent *= normalizedDifference / tempHeight;

        // calculate the difference in rotation
        // not a perfect solution as a square rotated 90 degrees shouldn't decrease precision
        float tempRotation = pieceRec.rotation_error.x_angle;
        normalizedDifference = tempRotation == 0f ? 1f : tempRotation;
        tempPercent *= normalizedDifference/360;
        tempRotation = pieceRec.rotation_error.y_angle;
        normalizedDifference = tempRotation == 0f ? 1f : tempRotation;
        tempPercent *= normalizedDifference/360;
        tempRotation = pieceRec.rotation_error.z_angle;
        normalizedDifference = tempRotation == 0f ? 1f : tempRotation;
        tempPercent *= normalizedDifference/360;

        return tempPercent;
    } */
}
