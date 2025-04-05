using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePositionDetector : SpatialNetworkBehaviour
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
    public GameObject pieceCaptured;
    public ArrayMap map;

    public Transform positionCapture;
        
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

    public void ObtainPieceCapture()
    {
        GiveControl();
        foreach (ArrayMap.Map item in map.maps)
        {
            if (item.occupiedBy!= null && item.name == currentLetter && item.number == currentNumber)
            {
                PieceType pieceType = GetComponent<PieceType>();
                PieceType pieceTypeOther = item.occupiedBy.GetComponent<PieceType>();
                if (pieceType.color != pieceTypeOther.color)
                {
                    pieceCaptured = item.occupiedBy;
                    Capture();
                    return;
                }
                
            }
            else
            {
                pieceCaptured = null;
            }
        }
    }

    public void Capture()
    {
        if (pieceCaptured)
        {
            pieceCaptured.GetComponent<PiecePositionDetector>().GiveControl();
            pieceCaptured.transform.position = pieceCaptured.GetComponent<PiecePositionDetector>().positionCapture.position;
            pieceCaptured = null;
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
    public void GiveControl()
    {
        SpatialNetworkObject obj = GetComponent<SpatialNetworkObject>();
        obj.RequestOwnership();
    }
}
