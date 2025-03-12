using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public List<GameObject> blancos = new List<GameObject>();
    public List<GameObject> negros = new List<GameObject>();
    public static Table instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        for (int i = 0; i < negros.Count; i++)
        {
            negros[i].GetComponent<SpatialInteractable>().enabled = false;
        }
    }

    public void TurnBlack()
    {
        for (int i = 0; i < negros.Count; i++)
        {
            negros[i].GetComponent<SpatialInteractable>().enabled = true;
        }
        for (int i = 0; i < blancos.Count; i++)
        {
            blancos[i].GetComponent<SpatialInteractable>().enabled = false;
        }
    }
    public void TurnWhite()
    {
        for (int i = 0; i < blancos.Count; i++)
        {
            blancos[i].GetComponent<SpatialInteractable>().enabled = true;
        }
        for (int i = 0; i < negros.Count; i++)
        {
            negros[i].GetComponent<SpatialInteractable>().enabled = false;
        }
    }
}
