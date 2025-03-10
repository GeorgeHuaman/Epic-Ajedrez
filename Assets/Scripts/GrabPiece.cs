using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPiece : SpatialNetworkBehaviour
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
            GiveControl(this.piece);
        }
        else
        {
            isGrab = false;
            this.piece = null;
        }
        
    }
    public void GiveControl(GameObject pieceNetwork)
    {
        if (!hasControl)
        {
            SpatialNetworkObject obj = pieceNetwork.GetComponent<SpatialNetworkObject>();
            obj.RequestOwnership();
        }

    }
}
