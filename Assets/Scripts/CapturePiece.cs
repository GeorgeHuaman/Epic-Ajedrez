using JetBrains.Annotations;
using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePiece : MonoBehaviour
{
    public List<ZoneCapture> captured = new List<ZoneCapture>();

    public void CapturedPiece(GameObject piece)
    {
        for (int i = 0; i < captured.Count; i++)
        {
            if (captured[i].ocupade == false)
            {
                piece.transform.position = captured[i].zone.transform.position;
                captured[i].ocupade = true;
                break;
            }
        }
    }
}

[Serializable]
public class ZoneCapture
{
    public GameObject zone;
    public bool ocupade;

    public ZoneCapture(GameObject zone, bool captured)
    {
        this.zone = zone;
        this.ocupade = captured;
    }
}
