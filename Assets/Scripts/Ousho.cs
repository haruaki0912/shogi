using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ousho : Piece
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[9, 9]; 

        KingMove(CurrentX + 1, CurrentY + 1, ref r);
        KingMove(CurrentX, CurrentY + 1, ref r);
        KingMove(CurrentX - 1, CurrentY + 1, ref r);
        KingMove(CurrentX + 1, CurrentY, ref r);
        KingMove(CurrentX - 1, CurrentY, ref r);
        KingMove(CurrentX - 1, CurrentY - 1, ref r);
        KingMove(CurrentX, CurrentY - 1, ref r);
        KingMove(CurrentX + 1, CurrentY - 1, ref r);


        return r; 
    }

    public void KingMove(int x, int y, ref bool[,] r)
    {
        if (x >= 0 && x < 9 && y >= 0 && y < 9)
        {
            Piece c = GameManager.Instance.Pieces[x, y];
            if (c == null)
                r[x, y] = true;
            else if (isBlack != c.isBlack)
                r[x, y] = true;
        }
    }
}
