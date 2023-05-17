using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprintsCollectionScript : MonoBehaviour
{

    // Lay objects on a plane in a grid, left to right, top to bottom, 4 by 3.
    public void Lay(Transform planeTransform){
        // Grid parameters, max Per Row is the number of times you can lay the biggest object on the plane completely
        // They are however arbitrary
        int maxPerColumn = 3;
        int maxPerRow = 4;
        // Calculate needed values
        float planeCenterX = planeTransform.GetComponent<Renderer>().bounds.center.x;
        float planeCenterZ = planeTransform.GetComponent<Renderer>().bounds.center.z;
        float planeLength = planeTransform.GetComponent<DimensionScript>().getGlobalLength();
        float planeDepth = planeTransform.GetComponent<DimensionScript>().getGlobalDepth();
        Transform biggest = getBiggest();
        float biggestLength = biggest.GetComponent<DimensionScript>().getGlobalLength();
        float biggestDepth = biggest.GetComponent<DimensionScript>().getGlobalDepth();
        // X Distance between first and last row should be the plane length with margins and based on the biggest object
        float spaceX = (planeLength - maxPerRow*biggestLength)/(maxPerRow+1);
        // Z Distance between first and last column should be of the plane depth with margins and based on the biggest object
        float spaceZ = (planeDepth - maxPerColumn*biggestDepth)/(maxPerColumn+1);
        // Shuffle the Children objects
        int numOfChildren = this.transform.childCount;
        ShuffleChildren(numOfChildren);
        // Calculate the centers
        List<float> listCenterX = new List<float>();
        List<float> listCenterZ = new List<float>();
        // to delete, imprints have 0 length and 0 depth
        print(biggestLength);
        print(biggestDepth);
        for (int column = 0; column < maxPerRow; column++){
            listCenterX.Add(planeCenterX - planeLength/2 + spaceX + biggestLength/2 + column * (spaceX+biggestLength));
        }
        for (int row = 0; row < maxPerColumn; row++){
            listCenterZ.Add(planeCenterZ + planeDepth/2 - spaceZ - biggestDepth/2 - row * (spaceZ+biggestDepth));
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
            tempColumn++;
            if(tempColumn >= maxPerRow){
                tempColumn = 0;
                tempRow++;
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

    // Get the Global surface of the X and Z bounds of the transform
    float getBoundingBox(Transform objectTransform){
        return objectTransform.GetComponent<DimensionScript>().getGlobalLength() * objectTransform.GetComponent<DimensionScript>().getGlobalDepth();
    }

    // Shuffle the hierarchy of a number of imprints
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
