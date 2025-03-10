using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPiece : MonoBehaviour
{
    private GameObject piece;
    private bool isGrab;

    private void Update()
    {
        if (isGrab)
            piece.transform.position = SpatialBridge.actorService.localActor.avatar.position;
            

    }
    public void Grab(GameObject piece)
    {
        if(!isGrab)
        {
            isGrab = true;
            this.piece = piece;
        }
        else
        {
            isGrab = false;
            this.piece = null;
        }
        
    }
}
