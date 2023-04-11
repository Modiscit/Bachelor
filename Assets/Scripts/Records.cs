[System.Serializable]

public class Records
{
// time from first grab to click terminate button
public float time_total;
// list of records of objects
public ObjectRecords[] objects;
// the parameters used for the task
public float scale;
public string scalemode;
public bool limit_to_field_of_view;
public string rotationmode;
public string[] colors;
public string colormode;
public bool PRL;
public string PRL_color;
public float PRL_size;
public int PRL_angle;
public float PRL_distance;
}
