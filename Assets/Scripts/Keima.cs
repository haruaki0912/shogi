using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keima : Piece
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[9, 9]; 

        if (!isPromote)
        {

            if (isBlack)
            {
                KeimaMove(CurrentX - 1, CurrentY + 2, ref r);
                KeimaMove(CurrentX + 1, CurrentY + 2, ref r);
            }
            else
            {
                KeimaMove(CurrentX - 1, CurrentY - 2, ref r);
                KeimaMove(CurrentX + 1, CurrentY - 2, ref r);
            }
        }
        else
        {
            if (isBlack)
            {
                KinshoMove(CurrentX + 1, CurrentY + 1, ref r);
                KinshoMove(CurrentX, CurrentY + 1, ref r);
                KinshoMove(CurrentX - 1, CurrentY + 1, ref r);
                KinshoMove(CurrentX + 1, CurrentY, ref r);
                KinshoMove(CurrentX - 1, CurrentY, ref r);
                KinshoMove(CurrentX, CurrentY - 1, ref r);
            }
            else
            {
                KinshoMove(CurrentX - 1, CurrentY - 1, ref r);
                KinshoMove(CurrentX, CurrentY - 1, ref r);
                KinshoMove(CurrentX + 1, CurrentY - 1, ref r);
                KinshoMove(CurrentX + 1, CurrentY, ref r);
                KinshoMove(CurrentX - 1, CurrentY, ref r);
                KinshoMove(CurrentX, CurrentY + 1, ref r);
            }
        }
        return r;
    }

    public void KeimaMove(int x, int y, ref bool[,] r)
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

    public void KinshoMove(int x, int y, ref bool[,] r)
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