using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePositionDetector : MonoBehaviour
{
    public string currentLetter;
    public int currentNumber;
    public string currentLetterBefore;
    public int currentNumberBefore;
    public List<(string, int)> positionPosible = new List<(string, int)>();
    public ArrayMap arrayMap;
    public GameObject groundFather;
    public Vector3 initialPosition;

    private void Start()
    {
        StartCoroutine(PositionInitial());
        initialPosition = transform.position;
    }

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
    void DetectStartPosition()
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

        if (closestLetter != currentLetterBefore || closestNumber != currentNumberBefore)
        {
            currentLetterBefore = closestLetter;
            currentNumberBefore = closestNumber;
        }
    }

    public void CorrectPosition()
    {
        Vector3 position = arrayMap.SearchGround(currentLetterBefore, currentNumberBefore);
        this.transform.position = position;
        ResetList();
    }
    public void ResetList()
    {
        positionPosible.Clear();
    }
    public string GetCurrentLetter()
    {
        return currentLetter;
    }

    public int GetCurrentNumber()
    {
        return currentNumber;
    }

    public bool VerifyPlay(GameObject piece)
    {
        for (int i = 0; i < positionPosible.Count; i++) {

            if (currentLetter == positionPosible[i].Item1 && currentNumber == positionPosible[i].Item2)
            {
                Debug.Log("HOLA");
                currentLetterBefore = currentLetter;
                currentNumberBefore = currentNumber;
                //arrayMap.CenterPiece(piece, currentLetter, currentNumber);
                return true;
            }
        }
        return false;
    }

    IEnumerator PositionInitial()
    {
        yield return new WaitForSeconds(0.5f);

        DetectStartPosition();
    }
}
