using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : SpatialNetworkBehaviour, IVariablesChanged
{
    public List<GameObject> blancos = new List<GameObject>();
    public List<GameObject> negros = new List<GameObject>();
    public static Table instance;

    private NetworkVariable<bool> turnWhite = new(initialValue: true);
    public Team team;
    public bool once;
    private void Start()
    {
        instance = this;
        //SelectWrite();
    }
    public void Update()
    {

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    NextTurn();
        //}
    }
    public void NextTurn()
    {
        GiveControl();
        if (turnWhite.value == true)
        {
            turnWhite.value = false;
            Timers.instance.StartBlackTimer();
        }
            
        else
        {
            turnWhite.value = true;
            Timers.instance.StartWhiteTimer();
        }
        
    }
    public void TurnForBlack()
    {
        GiveControl();
        turnWhite.value = false;
    }

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
    public void DisableAll()
    {
        foreach (GameObject go in blancos)
        {
            go.GetComponent<SpatialInteractable>().enabled = false;
        }

        foreach (GameObject go in negros)
        {
            go.GetComponent<SpatialInteractable>().enabled = false;
        }
    }
    public void GiveControl()
    {
        SpatialNetworkObject obj = GetComponent<SpatialNetworkObject>();
        obj.RequestOwnership();
    }

    public void OnVariablesChanged(NetworkObjectVariablesChangedEventArgs args)
    {
        if (args.changedVariables.ContainsKey(turnWhite.id))
        {
            if (!once)
            {
                once = true;
            }
            else
            {
                if (turnWhite.value == true)
                {
                    SelectWrite();
                }
                else
                {
                    SelectBlack();
                }
            }
        }
    }
}
