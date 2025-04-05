using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public bool isWhite;


    public void SelectWhite()
    {
        isWhite = true;
        Table.instance.SelectWrite();
    }
    public void SelectBlack()
    {
        isWhite = false;
    }
}
