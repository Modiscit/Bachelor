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

    public void Terminate(){
        GameObject Parameters = GameObject.Find("Parameters");
        Parameters.GetComponent<ParametersScript>().end = Time.time;
        Parameters.GetComponent<ParametersScript>().Terminate();
    }
}
