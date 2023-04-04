[System.Serializable]

public class Parameters
{
    // case sensitive variables matching the JSON file
    // the name of the user
    public string name;
    // whether or not the recorded data should be anonymous or not
    public bool anonimity;
    // a collection of object with names and integer, the names are those of the objects already in the program
    // there shouldn't be more than 12 objects due to the layout
    public Object[] objects;
    // a scale that should be between 1 and 0.1
    public float scale;
    // a string that is either "normal" or "imprintsonly"
    public string scalemode;
    // a string that is either "free", "xzlocked" or "locked"
    public string rotationmode;
    // a list of strings that correspond to names of materials
    public string[] colors;
    // a string that is either "normal", "blackandwhite" or "whiteandblack"
    public string colormode;
    // whether or not there should be a point for PRL
    public bool PRL;
    // the color of that point corresponding to a material's name
    public string PRL_color;
    // the diameter of the PRL in mm
    public float PRL_size;
    // the angle where the PRL is situated around the objects
    public int PRL_angle;
    // the distance from the end of the object
    public float PRL_distance;

}
