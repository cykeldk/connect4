using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerControl : MonoBehaviour, PlayerInterface
{

    private string playerName;
    private string playerColor;
    private string opponentColor;
    private BoardInterface board;
    private bool myTurn = false;
    private int nextCol = -1;

    // Use this for initialization
    public HumanPlayerControl(BoardInterface board, string name, string color)
    {
        this.board = board;
        this.playerColor = color;
        if (color.Equals("white"))
        {
            opponentColor = "black";
        }
        else
        {
            opponentColor = "white";
        }
    }

    

    public bool IsAi()
    {
        return false;
    }

    public string GetColor()
    {
        return playerColor;
    }

    public string GetName()
    {
        return playerName;
    }

    
    public bool PlayTurn()
    {
        myTurn = true;
        if (nextCol >= 0)
        {
            if (board.findRowInCol(nextCol) > 0)
            {
                board.AddPiece(nextCol, playerColor);
                nextCol = -1;
                myTurn = false;
                return true;

            }
        }
        int blockCol = canOtherPlayerWin();
        //Debug.Log(playerColor + "'s block col returned block column: " + blockCol);
        return false;
    }

    public void setNextCol(int col)
    {
        nextCol = col;
        Debug.Log("next col to play has been set to " + col);
    }

    public int canOtherPlayerWin()
    {

        for (int i = 0; i < board.GetHorizontalSize(); i++)
        {
            var tmpScore = board.checkCol(i, opponentColor);
            //Debug.Log("it was not " + playerColor + ", other players tmp score for " + i + " is " + tmpScore);
            if (tmpScore >= 3)
            {
                return i;
            }
        }
        return -1;
    }
}
