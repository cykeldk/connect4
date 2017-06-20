using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiControl : PlayerInterface {
    
    private string playerName;
    private string playerColor;

    private int confusion;
    private System.Random sysrand = new System.Random();
    private BoardInterface board;
    


    // Use this for initialization
    public AiControl (int confusion, BoardInterface board, string color) {
        this.confusion = confusion;
        this.board = board;
        this.playerColor = color;
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


        // add should i block other player method call here..
        var blockCol = canOtherPlayerWin();
        if (blockCol >= 0)
        {
            int block = sysrand.Next(100);
            if (block > confusion)
            {
                board.AddPiece(blockCol, playerColor);
                Debug.Log(playerName + " blocked the other players win :-)");
                return blockCol;
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
        for (int x = 0; x < board.GetHorizontalSize(); x++)
        {
            int y = board.findRowInCol(x) - 1;
            var pColor = board.GetPlayerAt(x, y);
            //Debug.Log("player at [" + x + "," + y + "] is " + pColor);
            if (pColor == null || pColor.Equals(playerColor)) continue;
            Debug.Log("it was not me, checking if i should block");

            if (board.checkCol(x, pColor) > 3)
            {
                Debug.Log("i should BLOCK");
                return x;
            }

        }
        return -1;
    }
}
