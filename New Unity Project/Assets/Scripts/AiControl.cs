using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiControl : PlayerInterface {
    
    private string playerName;
    private string playerColor;
    private string opponentColor;

    private int confusion;
    private System.Random sysrand = new System.Random();
    private BoardInterface board;

    // Use this for initialization
    public AiControl (int confusion, BoardInterface board, string color) {
        this.confusion = confusion;
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
	
	// Update is called once per frame

    public bool IsAi()
    {
        return true;
    }

    public string GetColor()
    {
        return playerColor;
    }

    public string GetName()
    {
        return "AI Level: " + (100-confusion);
    }

    
    public int PlayTurn()
    {
        
        List<int> possibleColsToPlay = new List<int>();
        float tempScore = 0f;
        var blockCol = canOtherPlayerWin();
        Debug.Log(playerColor + "'s block col returned block column: " + blockCol);
        if (blockCol >= 0)
        {
            int block = sysrand.Next(100);
            if (block > confusion)
            {
                board.AddPiece(blockCol, playerColor);
                Debug.Log(playerColor + " blocked the other players win :-)");
                return blockCol;
            }
            else
            {
                Debug.Log(playerColor + " got confused");
            }
        }
        
        

        for (int i = 0; i < board.GetHorizontalSize(); i++)
        {
            var score = board.checkCol(i, playerColor);

            if (score == tempScore)
            {
                // Debug.Log("adding " + i + " to possible cols");
                possibleColsToPlay.Add(i);
            }
            else if (score > tempScore)
            {
                // Debug.Log("score " + score + " is new highscore at " + i);
                possibleColsToPlay.Clear();
                possibleColsToPlay.Add(i);
                tempScore = board.checkCol(i, playerColor);
            }
            // Debug.Log("column " + i + " has a value of " + tempScore);
        }
        int indexToPlay = sysrand.Next(possibleColsToPlay.Count);
        //Debug.Log("amount: " + possibleColsToPlay.Count + " index chosen: " + possibleColsToPlay[indexToPlay] + " Highscore: " + tempScore);
        int colToPlay = possibleColsToPlay[indexToPlay];
        board.AddPiece(colToPlay, playerColor);

        //Debug.Log("______________________________________" + playerName + " is playing column " + colToPlay);
        return colToPlay;
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
