using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPiece : SpatialNetworkBehaviour, IVariablesChanged
{
    private NetworkVariable<bool> turnWhite = new(initialValue: false);
    private GameObject piece;
    private bool isGrab;
    public Table table;
    private Vector3 initialPiecePosition;
    private List<(string, int)> validMoves = new();

    [Header("GroundMaterials")]
    public Material defaultMaterial;
    public Material highlightMaterial;
    

    public ArrayMap map;
    private void Start()
    {
        table = Table.instance;
    }
    private void Update()
    {
        if (isGrab)
            piece.transform.position = SpatialBridge.actorService.localActor.avatar.position;


    }
    public void Grab(GameObject piece)
    {
        if (!isGrab)
        {
            initialPiecePosition = piece.transform.position;
            isGrab = true;
            this.piece = piece;
            GiveControl(this.piece);
            CalculatePieceMove();
        }
        else
        {
            TurnWhite(this.piece);
            //if (turnWhite)
            //    table.TurnBlack();

            //else
            //    table.TurnWhite();
            isGrab = false;
            this.piece = null;
            ResetHighlights();
            map.UpdateMapOccupancy();

        }
    }
    public void OnVariablesChanged(NetworkObjectVariablesChangedEventArgs args)
    {
        if (args.changedVariables.ContainsKey(turnWhite.id))
        {
            if (turnWhite)
                table.TurnBlack();

            else
                 table.TurnWhite();
        }
    }
    private void TurnWhite(GameObject piece)
    {
        GiveControlTurn();
        if (piece.GetComponent<PieceType>().color == PieceType.PieceColor.Blanco)
        {
            turnWhite.value = true;
        }
        else
        {
            turnWhite.value = false ;
        }
    }
    public void GiveControlTurn()
    {
        if (!hasControl)
        {
            SpatialNetworkObject obj = GetComponent<SpatialNetworkObject>();
            obj.RequestOwnership();
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
    void ResetHighlights()
    {
        foreach (var m in map.maps)
        {
            MeshRenderer mr = m.ground.GetComponent<MeshRenderer>();
            if (mr != null)
                mr.material = defaultMaterial;
        }
    }
    void HighlightGround(string letra, int numero)
    {
        foreach (var m in map.maps)
        {
            if (m.name == letra && m.number == numero)
            {
                MeshRenderer mr = m.ground.GetComponent<MeshRenderer>();
                if (mr != null)
                    mr.material = highlightMaterial;

                if (!validMoves.Contains((letra, numero)))
                    validMoves.Add((letra, numero));
            }
        }
    }

    public void CalculatePieceMove()
    {
        ResetHighlights();

        PieceType pieceType = piece.GetComponent<PieceType>();
        int direction = (pieceType.color == PieceType.PieceColor.Blanco) ? 1 : -1;

        switch (piece.GetComponent<PieceType>().type)
        {
            case PieceType.Type.Peon:
                PiecePositionDetector piecePositionDetector = piece.GetComponent<PiecePositionDetector>();
                string currentLetter = piecePositionDetector.currentLetter;
                int currentNumber = piecePositionDetector.currentNumber;

                int nextNumber = currentNumber + direction;

                if (IsGroundEmpty(currentLetter, nextNumber))
                {
                    HighlightGround(currentLetter, nextNumber);

                    if ((pieceType.color == PieceType.PieceColor.Blanco && currentNumber == 2) ||
                        (pieceType.color == PieceType.PieceColor.Negro && currentNumber == 7))
                    {
                        int doubleStep = currentNumber + (2 * direction);
                        if (IsGroundEmpty(currentLetter, doubleStep))
                        {
                            HighlightGround(currentLetter, doubleStep);
                        }
                    }
                }

                char currentLetterChar = currentLetter[0];
                char leftDiagonal = (char)(currentLetterChar - 1);
                char rightDiagonal = (char)(currentLetterChar + 1);

                if (leftDiagonal >= 'A' && leftDiagonal <= 'H')
                    CheckPawnCaptureMove(leftDiagonal.ToString(), nextNumber, pieceType.color);

                if (rightDiagonal >= 'A' && rightDiagonal <= 'H')
                    CheckPawnCaptureMove(rightDiagonal.ToString(), nextNumber, pieceType.color);
                break;

            case PieceType.Type.Torre:
                break;
            case PieceType.Type.Caballo:
                break;
            case PieceType.Type.Alfil:
                break;
            case PieceType.Type.Reina:
                break;
            case PieceType.Type.Rey:
                break;
            default:
                break;
        }
    }
    bool IsGroundEmpty(string letter, int number)
    {
        foreach (var m in map.maps)
        {
            if (m.name == letter && m.number == number)
            {
                return m.occupiedBy == null;
            }
        }
        return false;
    }

    #region CheckCapture
    void CheckPawnCaptureMove(string letter, int number, PieceType.PieceColor myColor)
    {
        foreach (var m in map.maps)
        {
            if (m.name == letter && m.number == number && m.occupiedBy != null)
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != myColor)
                {
                    MeshRenderer mr = m.ground.GetComponent<MeshRenderer>();
                    if (mr != null)
                        mr.material = highlightMaterial;
                }
            }
        }
    }
    #endregion

}
