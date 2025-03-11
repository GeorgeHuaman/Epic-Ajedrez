using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPiece : SpatialNetworkBehaviour
{
    private GameObject piece;
    private bool isGrab;

    [Header("GroundMaterials")]
    public Material defaultMaterial;
    public Material highlightMaterial;

    public ArrayMap map;
    private void Update()
    {
        if (isGrab)
            piece.transform.position = SpatialBridge.actorService.localActor.avatar.position;


    }
    public void Grab(GameObject piece)
    {
        if (!isGrab)
        {
            isGrab = true;
            this.piece = piece;
            GiveControl(this.piece);
            CalculatePieceMove();
        }
        else
        {
            isGrab = false;
            this.piece = null;
            ResetHighlights();
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

                int nextNumber = piecePositionDetector.currentNumber + direction;
                HighlightGround(piecePositionDetector.currentLetter, nextNumber);

                if ((pieceType.color == PieceType.PieceColor.Blanco && piecePositionDetector.currentNumber == 2) ||
                (pieceType.color == PieceType.PieceColor.Negro && piecePositionDetector.currentNumber == 7))
                {
                    HighlightGround(piecePositionDetector.currentLetter, piecePositionDetector.currentNumber + (2 * direction));
                }
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
}
