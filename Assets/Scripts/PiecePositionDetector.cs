using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PiecePositionDetector : SpatialNetworkBehaviour,IVariablesChanged
{
    public string currentLetter;
    public int currentNumber;
    public List<(string, int)> positionPosible = new List<(string, int)>();
    public ArrayMap arrayMap;
    public CapturePiece white;
    public CapturePiece black;
    public GameObject groundFather;
    public Vector3 initialPosition;
    public NetworkVariable<bool> inGame = new(initialValue: true);
    public GameObject pieceCaptured;
    public GrabPiece grabPiece;

    public Transform positionCapture;
        
    private void Start()
    {
        StartCoroutine(PositionInitial());
        initialPosition = transform.position;
        inGame.value = true;
    }

    void Update()
    {
        DetectCurrentPosition();
        if (GetComponent<PieceType>().type == PieceType.Type.Rey && inGame.value == false) 
        {
            switch (GetComponent<PieceType>().color)
            {
                case PieceType.PieceColor.Blanco:
                    PanelVictory.instance.BlackWins();
                    break;
                case PieceType.PieceColor.Negro:
                    PanelVictory.instance.WhiteWins();
                    break;
            }
        }

    }

    void DetectCurrentPosition()
    {
        //if (!inGame)
        //{
        //    currentLetter = string.Empty;
        //    currentNumber = 0;
        //}
        //else
        //{

        //    float minDistance = float.MaxValue;
        //    string closestLetter = "";
        //    int closestNumber = 0;

        //    foreach (Transform child in groundFather.transform)
        //    {
        //        Coordinate coord = child.GetComponent<Coordinate>();
        //        if (coord != null)
        //        {
        //            float distance = Vector3.Distance(transform.position, child.position);
        //            if (distance < minDistance)
        //            {
        //                minDistance = distance;
        //                closestLetter = coord.letra;
        //                closestNumber = coord.number;
        //            }
        //        }
        //    }

        //    if (closestLetter != currentLetter || closestNumber != currentNumber)
        //    {
        //        currentLetter = closestLetter;
        //        currentNumber = closestNumber;
        //    }

        //}
        float maxDetectDistance = 2f; // Distancia máxima que se permite para detectar una casilla

        if (!inGame.value)
        {
            currentLetter = string.Empty;
            currentNumber = 0;
        }
        else
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

            // Aquí revisamos si la pieza está lo suficientemente cerca de una casilla
            if (minDistance <= maxDetectDistance)
            {
                if (closestLetter != currentLetter || closestNumber != currentNumber)
                {
                    currentLetter = closestLetter;
                    currentNumber = closestNumber;
                }
            }
            else
            {
                // Si está demasiado lejos, puedes hacer algo aquí:
                Debug.Log("La pieza está fuera de rango de cualquier casilla");

                // Ejemplo: marcar que está fuera del tablero
                currentLetter = string.Empty;
                currentNumber = 0;
                if (hasControl)
                {
                    inGame.value = false;
                }
                

                // O llamar a otro método (como eliminarla o mostrar mensaje)
                // OutOfBoard();
            }
        }
    }

    public void ObtainPieceCapture()
    {
        GiveControl();
        foreach (ArrayMap.Map item in arrayMap.maps)
        {
            if (item.occupiedBy != null && item.name == currentLetter && item.number == currentNumber && grabPiece.validMoves.Contains((currentLetter, currentNumber)))
            //if (item.occupiedBy != null && item.name == currentLetter && item.number == currentNumber && grabPiece.letterCaptured == currentLetter && grabPiece.numberCaptured == currentNumber)
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
        GiveControl();
        if (pieceCaptured)
        {
            PiecePositionDetector piecePosCapture = pieceCaptured.GetComponent<PiecePositionDetector>();
            piecePosCapture.GiveControl();
            piecePosCapture.transform.position = piecePosCapture.positionCapture.position;
            piecePosCapture.inGame.value = false;
            arrayMap.UpdateMapOccupancy();
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
    public void EnablePiece()
    {
        GiveControl();
        inGame.value = true;
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

    public void OnVariablesChanged(NetworkObjectVariablesChangedEventArgs args)
    {
        if (args.changedVariables.ContainsKey(inGame.id))
        {
            arrayMap.UpdateMapOccupancy();
        }
    }
}
