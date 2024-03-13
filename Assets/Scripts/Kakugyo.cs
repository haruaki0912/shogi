using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kakugyo : Piece
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[9, 9]; // 9x9のボードに修正

        int i = CurrentX;
        int j = CurrentY;
        int still = 1;
        while (still > 0)
        {
            i--;
            j++;
            if (i < 0 || j >= 9)
            {
                break;
            }
            still = KakuMove(i, j, ref r, still);
        }

        i = CurrentX;
        j = CurrentY;
        still = 1;
        while (still > 0)
        {
            i++;
            j++;
            if (i >= 9 || j >= 9)
            {
                break;
            }
            still = KakuMove(i, j, ref r, still);
        }

        i = CurrentX;
        j = CurrentY;
        still = 1;
        while (still > 0)
        {
            i--;
            j--;
            if (i < 0 || j < 0)
            {
                break;
            }
            still = KakuMove(i, j, ref r, still);
        }

        i = CurrentX;
        j = CurrentY;
        still = 1;
        while (still > 0)
        {
            i++;
            j--;
            if (i >= 9 || j < 0)
            {
                break;
            }
            still = KakuMove(i, j, ref r, still);
        }

        if (isPromote)
        {
            UmaMove(CurrentX + 1, CurrentY, ref r);
            UmaMove(CurrentX, CurrentY + 1, ref r);
            UmaMove(CurrentX - 1, CurrentY, ref r);
            UmaMove(CurrentX, CurrentY - 1, ref r);
        }

        return r;
    }

    public int KakuMove(int x, int y, ref bool[,] r, int still)
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

    public void UmaMove(int x, int y, ref bool[,] r)
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
