using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePositionDetector : MonoBehaviour
{
    public string currentLetter;
    public int currentNumber;

    public GameObject groundFather;

    void Update()
    {
        DetectCurrentPosition();
    }

    void DetectCurrentPosition()
    {
        float minDistance = float.MaxValue;
        string closestLetter = "";
        int closestNumber = 0;

        foreach (Transform child in groundFather.transform)
        {
            Coordinate coord = child.GetComponent<Coordinate>();
            if (coord != null)
            {
                float distance = Vector3.Distance(transform.position, child.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestLetter = coord.letra;
                    closestNumber = coord.number;
                }
            }
        }

        if (closestLetter != currentLetter || closestNumber != currentNumber)
        {
            currentLetter = closestLetter;
            currentNumber = closestNumber;
        }
    }

    public string GetCurrentLetter()
    {
        return currentLetter;
    }

    public int GetCurrentNumber()
    {
        return currentNumber;
    }
}
