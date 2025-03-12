using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SpatialNetworkBehaviour
{
    public static GameManager instance;

    public bool isWhiteTurn = true;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void Turn()
    {

    }
    
}
