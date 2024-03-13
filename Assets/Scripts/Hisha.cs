using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hisha : Piece
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[9, 9]; 

        int i = CurrentX;
        int j = CurrentY;

        int still = 1;
        while (still > 0)
        {
            i++;
            if (i >= 9)
            {
                break;
            }
            still = HishaMove(i, j, ref r, still);
        }

        i = CurrentX;
        j = CurrentY;
        still = 1;
        while (still > 0)
        {
            i--;
            if (i < 0) 
            {
                break;
            }
            still = HishaMove(i, j, ref r, still);
        }

        i = CurrentX;
        j = CurrentY;
        still = 1;
        while (still > 0)
        {
            j++;
            if (j >= 9) 
            {
                break;
            }
            still = HishaMove(i, j, ref r, still);
        }

        i = CurrentX;
        j = CurrentY;
        still = 1;
        while (still > 0)
        {
            j--;
            if (j < 0) 
            {
                break;
            }
            still = HishaMove(i, j, ref r, still);
        }

        if (isPromote)
        {
            RyuMove(CurrentX + 1, CurrentY + 1, ref r);
            RyuMove(CurrentX - 1, CurrentY + 1, ref r);
            RyuMove(CurrentX - 1, CurrentY - 1, ref r);
            RyuMove(CurrentX + 1, CurrentY - 1, ref r);
        }

        return r;
    }

    public int HishaMove(int x, int y, ref bool[,] r, int still)
    {

        if (x >= 0 && x < 9 && y >= 0 && y < 9)
        {
            Piece c = GameManager.Instance.Pieces[x, y];
            if (c == null)
            {
                r[x, y] = true;
                still = 1;
            }
            else if (isBlack != c.isBlack)
            {
                r[x, y] = true;
                still = 0; 
            }
            else if (isBlack == c.isBlack)
            {
                still = 0; 
            }
        }
        return still;
    }
    public void RyuMove(int x, int y, ref bool[,] r)
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
