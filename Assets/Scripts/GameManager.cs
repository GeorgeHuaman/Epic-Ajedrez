using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SpatialNetworkBehaviour
{
    public static GameManager instance;

    private int isBlackTurn;
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
