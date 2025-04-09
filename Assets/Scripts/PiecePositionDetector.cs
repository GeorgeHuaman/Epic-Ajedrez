using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePositionDetector : MonoBehaviour
{
    public string currentLetter;
    public int currentNumber;
    public List<(string, int)> positionPosible = new List<(string, int)>();
    public ArrayMap arrayMap;
    public CapturePiece white;
    public CapturePiece black;
    public GameObject groundFather;
    public Vector3 initialPosition;
    public bool life;

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

    private void OutTable()
    {
        PieceType.PieceColor colors = gameObject.GetComponent<PieceType>().color;
        if (colors == PieceType.PieceColor.Blanco)
        {
            white.CapturedPiece(this.gameObject);
        }
        else
        {
            black.CapturedPiece(this.gameObject);
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

    }

    public string GetCurrentLetter()
    {
        return currentLetter;
    }

    public int GetCurrentNumber()
    {
        return currentNumber;
    }

    public void VerifyPlay(GameObject piece)
    {
       //arrayMap.CenterPiece(piece, currentLetter, currentNumber);
    }

    IEnumerator PositionInitial()
    {
        yield return new WaitForSeconds(0.5f);

        DetectStartPosition();
    }

}
