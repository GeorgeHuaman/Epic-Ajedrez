using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public List<GameObject> blancos = new List<GameObject>();
    public List<GameObject> negros = new List<GameObject>();
    public static Table instance;


    public void SelectBlack()
    {
        foreach (GameObject go in negros)
        {
            go.GetComponent<SpatialInteractable>().enabled = true;
        }
        foreach (GameObject go in blancos)
        {
            go.GetComponent<SpatialInteractable>().enabled = false;
        }
    }
    public void SelectWrite()

    {
        foreach (GameObject go in blancos)
        {
            go.GetComponent<SpatialInteractable>().enabled = true;
        }

        foreach (GameObject go in negros)
        {
            go.GetComponent<SpatialInteractable>().enabled = false;
        }
    }

}
