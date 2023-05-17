using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminationMenuScript : MonoBehaviour
{
    // Quit the application
    public void Quit(){
        Application.Quit();
    }

    // Reload the active Scene (there is just one scene so far)
    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Save the time for the end of the task
    // Call the terminate function of ParametersScript, rendering all pieces uninteractable and saving data
    public void Terminate(){
        GameObject Parameters = GameObject.Find("Parameters");
        Parameters.GetComponent<ParametersScript>().end = Time.time;
        Parameters.GetComponent<ParametersScript>().Terminate();
    }
}
