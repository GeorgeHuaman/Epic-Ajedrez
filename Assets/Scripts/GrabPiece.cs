﻿using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GrabPiece : SpatialNetworkBehaviour, IVariablesChanged
{
    public Team team;
    private GameObject piece;
    private bool isGrab;
    public Table table;
    private Vector3 initialPiecePosition;
    private List<(string, int)> validMoves = new();
    private string letterCaptured;
    private int numberCaptured;

    [Header("GroundMaterials")]
    public Material defaultMaterial;
    public Material highlightMaterial;
    public Material CapturedlightMaterial;
    public PiecePositionDetector positionDetector;
    

    public ArrayMap map;
    private bool once = true;

    private void Update()
    {
        if (isGrab)
            piece.transform.position = SpatialBridge.actorService.localActor.avatar.position;


    }
    public void Grab(GameObject piece)
    {
        if (once && team.isWhite)
        {
            Timers.instance.StartWhiteTimer();
            once = false;
        }
        if (!isGrab)
        {
            this.piece = piece;
            GiveControl(this.piece);
            map.UpdateMapOccupancy();
            initialPiecePosition = piece.transform.position;
            isGrab = true;
            CalculatePieceMove();
        }
        else
        {
            GiveControl(this.piece);
            piece.GetComponent<PiecePositionDetector>().VerifyPlay(piece);
            isGrab = false;
            ResetHighlights();
            piece.GetComponent<PiecePositionDetector>().ObtainPieceCapture();
            map.UpdateMapOccupancy();
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
                CheckPawnMove(pieceType, direction);
                break;

            case PieceType.Type.Torre:
                CheckRookMove(pieceType);
                break;
            case PieceType.Type.Caballo:
                CheckKnightMove(pieceType);
                break;
            case PieceType.Type.Alfil:
                CheckBishopMove(pieceType);
                break;
            case PieceType.Type.Reina:
                CheckQueenMove(pieceType);
                break;
            case PieceType.Type.Rey:
                CheckKingMove(pieceType);
                break;
            default:
                break;
        }
    }

    #region CheckPiecesMove()
    private void CheckPawnMove(PieceType pieceType, int direction)
    {
        //Movimiento
        PiecePositionDetector piecePositionDetector = piece.GetComponent<PiecePositionDetector>();
        string currentLetter = piecePositionDetector.currentLetter;
        int currentNumber = piecePositionDetector.currentNumber;

        int nextNumber = currentNumber + direction;

        if (map.IsGroundEmpty(currentLetter, nextNumber))
        {
            HighlightGround(currentLetter, nextNumber);

            if ((pieceType.color == PieceType.PieceColor.Blanco && currentNumber == 2) ||
                (pieceType.color == PieceType.PieceColor.Negro && currentNumber == 7))
            {
                int doubleStep = currentNumber + (2 * direction);
                if (map.IsGroundEmpty(currentLetter, doubleStep))
                {
                    HighlightGround(currentLetter, doubleStep);
                }
            }
        }
        //captura
        char currentLetterChar = currentLetter[0];
        char leftDiagonal = (char)(currentLetterChar - 1);
        char rightDiagonal = (char)(currentLetterChar + 1);

        if (leftDiagonal >= 'A' && leftDiagonal <= 'H')
            CheckCaptureMove(leftDiagonal.ToString(), nextNumber, pieceType.color);

        if (rightDiagonal >= 'A' && rightDiagonal <= 'H')
            CheckCaptureMove(rightDiagonal.ToString(), nextNumber, pieceType.color);
    }
    private void CheckRookMove(PieceType pieceType)
    {
        PiecePositionDetector piecePositionDetector = piece.GetComponent<PiecePositionDetector>();
        string currentLetter = piecePositionDetector.currentLetter;
        int currentNumber = piecePositionDetector.currentNumber;
        char currentLetterChar = currentLetter[0];

        //arriba
        for (int i = currentNumber + 1; i <= 8; i++)
        {
            var m = map.GetMap(currentLetter, i);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(currentLetter, i);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(currentLetter, i);
                }
                break;
            }
        }

        //abajo
        for (int i = currentNumber - 1; i >= 1; i--)
        {
            var m = map.GetMap(currentLetter, i);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(currentLetter, i);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(currentLetter, i);
                }
                break;
            }
        }

        //derecha
        for (char c = (char)(currentLetterChar + 1); c <= 'H'; c++)
        {
            var m = map.GetMap(c.ToString(), currentNumber);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(c.ToString(), currentNumber);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(c.ToString(), currentNumber);
                }
                break;
            }
        }

        //izquierda
        for (char c = (char)(currentLetterChar - 1); c >= 'A'; c--)
        {
            var m = map.GetMap(c.ToString(), currentNumber);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(c.ToString(), currentNumber);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(c.ToString(), currentNumber);
                }
                break;
            }
        }
    }
    private void CheckKnightMove(PieceType pieceType)
    {
        PiecePositionDetector piecePositionDetector = piece.GetComponent<PiecePositionDetector>();
        string currentLetter = piecePositionDetector.currentLetter;
        int currentNumber = piecePositionDetector.currentNumber;
        char currentLetterChar = currentLetter[0];

        // Definir los 8 posibles movimientos en forma de L
        List<(int, int)> offsets = new()
        {
            (2, 1), (2, -1), (-2, 1), (-2, -1),
            (1, 2), (1, -2), (-1, 2), (-1, -2)
        };

        foreach (var offset in offsets)
        {
            int targetNumber = currentNumber + offset.Item2;
            char targetLetterChar = (char)(currentLetterChar + offset.Item1);

            if (targetLetterChar >= 'A' && targetLetterChar <= 'H' &&
                targetNumber >= 1 && targetNumber <= 8)
            {
                var m = map.GetMap(targetLetterChar.ToString(), targetNumber);
                if (m != null)
                {
                    // ver si hay una pieza en la casilla
                    if (m.occupiedBy == null)
                    {
                        HighlightGround(targetLetterChar.ToString(), targetNumber);
                    }
                    else
                    {
                        PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                        if (otherPiece != null && otherPiece.color != pieceType.color)
                        {
                            HighlightGroundCaptured(targetLetterChar.ToString(), targetNumber); // ilumina posible captura
                        }
                    }
                }
            }
        }
    }
    private void CheckBishopMove(PieceType pieceType)
    {
        PiecePositionDetector piecePositionDetector = piece.GetComponent<PiecePositionDetector>();
        string currentLetter = piecePositionDetector.currentLetter;
        int currentNumber = piecePositionDetector.currentNumber;
        char currentLetterChar = currentLetter[0];

        // Movimiento: Diagonal arriba-derecha
        for (int i = 1; i < 8; i++)
        {
            char letter = (char)(currentLetterChar + i);
            int number = currentNumber + i;

            if (letter > 'H' || number > 8) break;

            var m = map.GetMap(letter.ToString(), number);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(letter.ToString(), number);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(letter.ToString(), number); // posible captura
                }
                break;
            }
        }

        // Diagonal arriba-izquierda
        for (int i = 1; i < 8; i++)
        {
            char letter = (char)(currentLetterChar - i);
            int number = currentNumber + i;

            if (letter < 'A' || number > 8) break;

            var m = map.GetMap(letter.ToString(), number);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(letter.ToString(), number);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(letter.ToString(), number);
                }
                break;
            }
        }

        // Diagonal abajo-derecha
        for (int i = 1; i < 8; i++)
        {
            char letter = (char)(currentLetterChar + i);
            int number = currentNumber - i;

            if (letter > 'H' || number < 1) break;

            var m = map.GetMap(letter.ToString(), number);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(letter.ToString(), number);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(letter.ToString(), number);
                }
                break;
            }
        }

        // Diagonal abajo-izquierda
        for (int i = 1; i < 8; i++)
        {
            char letter = (char)(currentLetterChar - i);
            int number = currentNumber - i;

            if (letter < 'A' || number < 1) break;

            var m = map.GetMap(letter.ToString(), number);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(letter.ToString(), number);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(letter.ToString(), number);
                }
                break;
            }
        }
    }
    public void CheckQueenMove(PieceType pieceType)
    {
        PiecePositionDetector piecePositionDetector = piece.GetComponent<PiecePositionDetector>();
        string currentLetter = piecePositionDetector.currentLetter;
        int currentNumber = piecePositionDetector.currentNumber;
        char currentLetterChar = currentLetter[0];
        //arriba
        for (int i = currentNumber + 1; i <= 8; i++)
        {
            var m = map.GetMap(currentLetter, i);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(currentLetter, i);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(currentLetter, i);
                }
                break;
            }
        }

        //abajo
        for (int i = currentNumber - 1; i >= 1; i--)
        {
            var m = map.GetMap(currentLetter, i);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(currentLetter, i);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(currentLetter, i);
                }
                break;
            }
        }

        //derecha
        for (char c = (char)(currentLetterChar + 1); c <= 'H'; c++)
        {
            var m = map.GetMap(c.ToString(), currentNumber);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(c.ToString(), currentNumber);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(c.ToString(), currentNumber);
                }
                break;
            }
        }

        //izquierda
        for (char c = (char)(currentLetterChar - 1); c >= 'A'; c--)
        {
            var m = map.GetMap(c.ToString(), currentNumber);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(c.ToString(), currentNumber);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(c.ToString(), currentNumber);
                }
                break;
            }
        }
        // Movimiento: Diagonal arriba-derecha
        for (int i = 1; i < 8; i++)
        {
            char letter = (char)(currentLetterChar + i);
            int number = currentNumber + i;

            if (letter > 'H' || number > 8) break;

            var m = map.GetMap(letter.ToString(), number);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(letter.ToString(), number);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(letter.ToString(), number); // posible captura
                }
                break;
            }
        }

        // Diagonal arriba-izquierda
        for (int i = 1; i < 8; i++)
        {
            char letter = (char)(currentLetterChar - i);
            int number = currentNumber + i;

            if (letter < 'A' || number > 8) break;

            var m = map.GetMap(letter.ToString(), number);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(letter.ToString(), number);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(letter.ToString(), number);
                }
                break;
            }
        }

        // Diagonal abajo-derecha
        for (int i = 1; i < 8; i++)
        {
            char letter = (char)(currentLetterChar + i);
            int number = currentNumber - i;

            if (letter > 'H' || number < 1) break;

            var m = map.GetMap(letter.ToString(), number);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(letter.ToString(), number);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(letter.ToString(), number);
                }
                break;
            }
        }

        // Diagonal abajo-izquierda
        for (int i = 1; i < 8; i++)
        {
            char letter = (char)(currentLetterChar - i);
            int number = currentNumber - i;

            if (letter < 'A' || number < 1) break;

            var m = map.GetMap(letter.ToString(), number);
            if (m == null) break;

            if (m.occupiedBy == null)
            {
                HighlightGround(letter.ToString(), number);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(letter.ToString(), number);
                }
                break;
            }
        }
    }
    public void CheckKingMove(PieceType pieceType)
    {
        PiecePositionDetector piecePositionDetector = piece.GetComponent<PiecePositionDetector>();
        string currentLetter = piecePositionDetector.currentLetter;
        int currentNumber = piecePositionDetector.currentNumber;
        char currentLetterChar = currentLetter[0];

        List<(int, int)> direction = new ()
        { (1,0), (-1,0), (0,1), (0,-1), (1,1),(-1,1),(1,-1),(-1,-1)};

        foreach (var dir in direction) 
        { 
          char targetLetterchar = (char)(currentLetterChar+ dir.Item1);
          int targetNumber = currentNumber + dir.Item2;
            if(targetLetterchar < 'A' ||  targetLetterchar > 'H' || targetNumber < 1 || targetNumber > 8)
            {
                continue;

            }
            var m = map.GetMap(targetLetterchar.ToString(), targetNumber);
            if (m == null) {continue;}
            if (m.occupiedBy == null)
            {
                HighlightGround(targetLetterchar.ToString(), targetNumber);
            }
            else
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != pieceType.color)
                {
                    HighlightGroundCaptured(targetLetterchar.ToString(), targetNumber); // ilumina posible captura
                }
            }
        }
    }
    #endregion
    #region NetworkControl
    public void GiveControlTurn()
    {
            SpatialNetworkObject obj = GetComponent<SpatialNetworkObject>();
            obj.RequestOwnership();

    }
    public void GiveControl(GameObject pieceNetwork)
    {
            SpatialNetworkObject grab = GetComponent<SpatialNetworkObject>();
            grab.RequestOwnership();
            SpatialNetworkObject obj = pieceNetwork.GetComponent<SpatialNetworkObject>();
            obj.RequestOwnership();

    }
    public void OnVariablesChanged(NetworkObjectVariablesChangedEventArgs args)
    {
    }
    #endregion

    #region Highlight
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
                {
                    mr.material = highlightMaterial;
                    piece.GetComponent<PiecePositionDetector>().positionPosible.Add((letra,numero));
                }

                if (!validMoves.Contains((letra, numero)))
                    validMoves.Add((letra, numero));
            }
        }
    }
    void HighlightGroundCaptured(string letra, int numero)
    {
        letterCaptured = letra;
        numberCaptured = numero;
        foreach (var m in map.maps)
        {
            if (m.name == letra && m.number == numero)
            {
                MeshRenderer mr = m.ground.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.material = CapturedlightMaterial;
                    piece.GetComponent<PiecePositionDetector>().positionPosible.Add((letra, numero));
                }

                if (!validMoves.Contains((letra, numero)))
                    validMoves.Add((letra, numero));
            }
        }
    }
    #endregion

    #region CheckCapture
    void CheckCaptureMove(string letter, int number, PieceType.PieceColor myColor)
    {
        letterCaptured = letter;
        numberCaptured = number;
        foreach (var m in map.maps)
        {
            if (m.name == letter && m.number == number && m.occupiedBy != null)
            {
                PieceType otherPiece = m.occupiedBy.GetComponent<PieceType>();
                if (otherPiece != null && otherPiece.color != myColor)
                {
                    MeshRenderer mr = m.ground.GetComponent<MeshRenderer>();
                    piece.GetComponent<PiecePositionDetector>().positionPosible.Add((letter, number));
                    if (mr != null)
                        mr.material = CapturedlightMaterial;
                }
            }
        }
    }
    #endregion

    public void ResetPosition()
    {
        foreach (var whrite in table.blancos)
        {
            if (whrite != null)
            {
                GiveControl(whrite);
                whrite.transform.position = whrite.GetComponent<PiecePositionDetector>().initialPosition;
            }
        }
        foreach (var black in table.negros)
        {
            if (black != null)
            {
                GiveControl(black);
                black.transform.position = black.GetComponent<PiecePositionDetector>().initialPosition;
            }
        }
        GiveControlTurn();
    }

}
