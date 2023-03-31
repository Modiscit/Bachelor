using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsCollectionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Lay objects next to a plane in a grid, bottom to top, right to left, 4 by 3.
    public void Lay(Transform planeTransform){
        // Grid parameters, max Per Row is the number of times you can lay the biggest object on the plane completely
        // They are however arbitrary
        int maxPerColumn = 4;
        int maxPerRow = 3;
        // Calculate needed values
        float planeCenterX = planeTransform.GetComponent<Renderer>().bounds.center.x;
        float planeCenterZ = planeTransform.GetComponent<Renderer>().bounds.center.z;
        float planeHalfLength = planeTransform.GetComponent<DimensionScript>().getGlobalLength() / 2;
        float planeDepth = planeTransform.GetComponent<DimensionScript>().getGlobalDepth();
        Transform biggest = getBiggest();
        float biggestLength = biggest.GetComponent<DimensionScript>().getGlobalLength();
        float biggestDepth = biggest.GetComponent<DimensionScript>().getGlobalDepth();
        // X Distance between first and last row should be half of the plane length (arbitrary) and based on the biggest object
        float spaceX = (planeHalfLength - maxPerRow*biggestLength)/(maxPerRow);
        // Z Distance between first and last column should be of the plane depth (arbitrary) and based on the biggest object (which can be layed three times on the plane)
        float spaceZ = (planeDepth - (maxPerColumn-1)*biggestDepth)/(maxPerColumn-1);
        // Shuffle the Children objects
        int numOfChildren = this.transform.childCount;
        ShuffleChildren(numOfChildren);
        // Calculate the centers
        List<float> listCenterX = new List<float>();
        List<float> listCenterZ = new List<float>();
        for (int column = 0; column < maxPerRow; column++){
            listCenterX.Add(planeCenterX - planeHalfLength - (column+1) * (spaceX+biggestLength));
        }
        for (int row = 0; row < maxPerColumn; row++){
            listCenterZ.Add(planeCenterZ + planeDepth/2 - row * (spaceZ+biggestDepth));
        }
        int tempRow = 0;
        int tempColumn = 0;
        foreach (Transform child in transform){
            Vector3 newPosition = child.position;
            // the center is not necessarily the origin in Unity
            float biasX = child.position.x - child.GetComponent<Renderer>().bounds.center.x;
            float biasZ = child.position.z - child.GetComponent<Renderer>().bounds.center.z;
            newPosition.x = listCenterX[tempColumn] + biasX;
            newPosition.z = listCenterZ[tempRow] + biasZ;
            child.position = newPosition;
            tempRow++;
            if(tempRow >= maxPerColumn){
                tempRow = 0;
                tempColumn++;
            }
        }
    }

    // Get the object that has the biggest x-z area bounding box
    Transform getBiggest(){
        Transform biggest = null;
        foreach (Transform child in transform){
            if (biggest == null){
                biggest = child;
            }
            else if (getBoundingBox(child) > getBoundingBox(biggest)){
                biggest = child;
            }
        }
        return biggest;
    }

    float getBoundingBox(Transform objectTransform){
        return objectTransform.GetComponent<DimensionScript>().getGlobalLength() * objectTransform.GetComponent<DimensionScript>().getGlobalDepth();
    }

    void ShuffleChildren(int numOfChildren){
        List<Transform> childList = new List<Transform>();
        List<int> childIndexList = new List<int>();
        int tempindex = 0;
        // add the children to the list for access later and fill the other list with index number
        foreach (Transform child in transform)
        {
            childIndexList.Add(tempindex++);
            childList.Add(child);
        }
        // Shuffle the child indices list using Fisher-Yates algorithm
        for (int i = numOfChildren - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = childIndexList[i];
            childIndexList[i] = childIndexList[j];
            childIndexList[j] = temp;
        }
        // Move the child objects to their shuffled indices
        // Finds the index of the first to last objet and puts them last in the hierarchy from first to last.
        for (int i = 0; i < numOfChildren; i++)
        {
            childList[childIndexList.IndexOf(i)].SetAsLastSibling();
        }
    }

}