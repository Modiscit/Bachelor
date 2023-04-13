[System.Serializable]

public class ObjectRecords
{
    // type of the object mesh
    public string type;
    // color of the object
    public string color;
    // number of errors recorded for that object
    public int number_of_errors;
    // number of errors of color recorded for that object
    public int number_of_color_errors;
    // number of errors of shape recorded for that object
    public int number_of_shape_errors;
    // time spent interacting with that object, first grab to last release
    public float time_interacting;
    // difference of direction x y z between the center of the object to the center of the imprint
    public DirectionRecords direction_of_error;
    // difference of rotation x y z between the object and the imprint
    public RotationRecords rotation_error;

}
