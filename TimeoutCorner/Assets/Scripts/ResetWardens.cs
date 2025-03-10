using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetWardens : MonoBehaviour
{
    private GameObject[] CubeWardens; // Store references

    void Start()
    {
        CubeWardens = GameObject.FindGameObjectsWithTag("WardenCube"); // Initialize once
    }

    public void ReactivateWarden() 
    {
        foreach (GameObject warden in CubeWardens) 
        {
            DefenseDestroy defenseScript = warden.GetComponent<DefenseDestroy>();
            if (defenseScript != null) 
            {
                defenseScript.ReactivateObject(); // Call the method on each warden
            }
        }
    }
}
