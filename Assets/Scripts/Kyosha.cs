using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kyosha : Piece
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[9, 9]; 

        int i = CurrentY;
        int still = 1;

        if (!isPromote)
        {
            if (isBlack)
            {
                while (still > 0)
                {
                    i++;
                    if (i > 9)
                    {
                        break;
                    }
                    still = KyoshaMove(CurrentX, i, ref r, still);
                }
            }
            else
            {
                while (still > 0)
                {
                    i--;
                    if (i < 0)
                    {
                        break;
                    }
                    still = KyoshaMove(CurrentX, i, ref r, still);
                }
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

    public int KyoshaMove(int x, int y, ref bool[,] r, int still)
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
