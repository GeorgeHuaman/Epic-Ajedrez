using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceType : MonoBehaviour
{
    public enum Type
    {
        Peon,
        Torre,
        Caballo,
        Alfil,
        Reina,
        Rey
    }

    public enum PieceColor
    {
        Blanco,
        Negro
    }
    [Header("Chess Piece Info")]
    public Type type;
    public PieceColor color;
    public int move = 0;

    
}
