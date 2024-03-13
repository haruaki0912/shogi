using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public int pieceIndex;
    public bool isPromote { set; get; }
    public bool canPromote { set; get; }
    public bool isBlack;



    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[9, 9];
    }

    public void SetPieceIndex(int index)
    {
        pieceIndex = index;
    }
}
