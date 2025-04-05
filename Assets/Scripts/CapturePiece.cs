using JetBrains.Annotations;
using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePiece : MonoBehaviour
{
    public List<ZoneCapture> captured = new List<ZoneCapture>();
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
