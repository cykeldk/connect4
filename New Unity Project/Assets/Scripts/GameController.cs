using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject FieldPrefab;
    public GameObject white;
    public GameObject black;
    public int VerticalSize;
    public int HorizontalSize;
    public float fieldSize;
    public int scoreToWin;
    private FieldInfo[][] fields;
    private float horizontalOffset = 2f;
    private float verticalOffset = -4f;
    private System.Random sysrand = new System.Random();
    private int[][] directions;
    public int Confusion;
    public string currentPlayer;
    public string otherPlayer;
    private bool gameOver = false;

    // variables used for testing
    public int addPieces;
    public bool colsTested = false;
    public int[] colsToTest = {5};
    


    // Use this for initialization
    void Start () {
        directions = createDirections();
        fields = new FieldInfo[HorizontalSize][];
        for ( int i = 0; i < HorizontalSize; i++)
        {
            fields[i] = new FieldInfo[VerticalSize];
        }
		for (int x = 0; x < HorizontalSize; x++)
        {
            for (int y = 0; y < VerticalSize; y++)
            {
                FieldInfo fo = new FieldInfo(x, y, FieldPrefab);
                
                fields[x][y] = fo;
            }
        }
	}

    // Update is called once per frame
    void Update() {
        if (!gameOver)
        {

            int lastCol = PlayTurn(currentPlayer);
            if (checkWin(lastCol, currentPlayer))
            {
                Debug.Log("------------------------------------------------" + currentPlayer + " wins");
                gameOver = true;
                // make UI tell player that game is over, and ask for another game..
            }
            else
            {
                togglePlayer();
            }
        }
        
        //if (currentPlayer.Equals("white"))
        //{
        //    int lastCol = PlayTurn();

        //    if (checkWin(lastCol))
        //    {
        //        Debug.Log("------------------------------------------------" + currentPlayer + " wins");
        //    }
        //    else
        //    {
        //        currentPlayer = "black";
        //    }
        //}
        //else
        //{
        //    int lastCol = PlayTurn();
        //    if (checkWin(lastCol))
        //    {
        //        Debug.Log("------------------------------------------------" + currentPlayer + " wins");
        //    }
        //    currentPlayer = "white";
        //}
    }

    public int PlayTurn(string playerName)
    {
        List<int> possibleColsToPlay = new List<int>();
        float tempScore = 0f;


        // add should i block other player method call here..
        var blockCol = canOtherPlayerWin();
        if (blockCol >= 0)
        {
            int block = sysrand.Next(100);
            if (block > Confusion)
            {
                Debug.Log(currentPlayer + " blocked " + otherPlayer);
                AddPiece(blockCol, currentPlayer);
                return blockCol;
            }
        }
           
        for (int i = 0; i < HorizontalSize; i++)
        {
            var score = checkCol(i, playerName);

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
                tempScore = checkCol(i, playerName);
            }
            // Debug.Log("column " + i + " has a value of " + tempScore);
        }
        int indexToPlay = sysrand.Next(possibleColsToPlay.Count);
        Debug.Log("amount: " + possibleColsToPlay.Count + " index chosen: " + possibleColsToPlay[indexToPlay] + " Highscore: " + tempScore);
        int colToPlay = possibleColsToPlay[indexToPlay];
        AddPiece(colToPlay, playerName);
        
        Debug.Log("______________________________________" + playerName + " is playing column " + colToPlay);
        return colToPlay;
    }

    public int canOtherPlayerWin()
    {
        for (int i = 0; i < HorizontalSize; i++)
        {
            if (checkCol(i, otherPlayer) > 3) return i;
        }
        return -1;
    }

    public void AddPiece(int col, string player)
    {
        int row = findRowInCol(col);
        addPiece(col, row, player);
        
    }

    private bool addPiece(int col, int row, string player)
    {
        
        GameObject go;
        if (player.Equals("black")) go = black;
        else if (player.Equals("white")) go = white;
        else throw new System.Exception("Fucktard, you failed to specify or spell which player it is!!");
        if (row >= VerticalSize) return false;
        if (col >= HorizontalSize) return false;
        if (fields[col][row].GetPiece()==null)
        {
            fields[col][row].SetPiece(go, player);
            return true;
        }
        return addPiece(col, row + 1, player);
    }

    private bool checkWin(int col, string playerName)
    {
        // Debug.Log("checking for winner on col" + col);
        for (int i = 0; i < directions.Length / 2; i++)
        {
            var direction1 = directions[i];
            var direction2 = directions[directions.Length - (i + 1)];
            var scoreDirection1 = checkScoreFromPoint(1, col, findRowInCol(col) - 1, direction1[0], direction1[1], playerName);
            var scoreDirection2 = checkScoreFromPoint(1, col, findRowInCol(col) - 1, direction2[0], direction2[1], playerName);
            var tempScore =  (scoreDirection1 + scoreDirection2 - 1);
            // Debug.Log("Direction: [x:" + direction[0] + "; y: " + direction[1] + "]");
            //Debug.Log("score direction1: " + scoreDirection1);
            //Debug.Log("score opposite: " + scoreDirection2);
            if (tempScore >= scoreToWin)
            {
                Debug.Log(playerName + " is the winner");
                return true;
            }
        }
        return false;
    }

    private float checkCol(int col, string playerName)
    {
        
        if (fields[col][VerticalSize - 1].GetPiece()!=null)
        {
            // Debug.Log("column " + col + " is full");
            return 0f;
        }
        float tmp = 0f;
        string debugString = "column " + col + "\n";
        for (int i = 0; i < directions.Length / 2; i++)
        {
            var row = findRowInCol(col);
            var direction1 = directions[i];
            var direction2 = directions[directions.Length - (i + 1)];
            
            var scoreDirection1 = checkScoreFromPoint(1, col, row, direction1[0], direction1[1], playerName);
            var scoreDirection2 = checkScoreFromPoint(1, col, row, direction2[0], direction2[1], playerName);

            debugString += i + "(" + direction1[0] + ", " + direction1[1] + ")  ::: score = " + scoreDirection1 + "\n";
            debugString += i + "(" + direction2[0] + ", " + direction2[1] + ")  ::: score = " + scoreDirection2 + "\n";
            debugString += "Sum : " + (scoreDirection1 + scoreDirection2) + "\n";

            var tempScore = (scoreDirection1 + scoreDirection2 - 1); // minus one because checkScore from point counts the field it starts on
            
            if (tempScore > tmp)
            {
                tmp = tempScore;
            }
        }
        // Debug.Log(debugString);
        debugString = "";
        return tmp;

        /*
        int row = findRowInCol(col);
        res += scoreForCenter(col, row);
        res += checkVertical(col, row);
        res += checkHorizontal(col, row, true) + checkHorizontal(col, row, false);
        res += checkDiagonalUp(col, row, true) + checkDiagonalUp(col, row, false);
        res += checkDiagonalDown(col, row, true) + checkDiagonalDown(col, row, false);
        return 1 - 1 / res;
        */
    }

    private int findRowInCol(int col)
    {
        
        int counter = 0;
        while (counter < VerticalSize && fields[col][counter].GetPiece()!=null)
        {
            counter++;
        }
        return counter;
    }

    
    private int checkScoreFromPoint(int count, int positionX, int positionY, int directionX, int directionY, string playerName)
    {
        
        int nextX = positionX + directionX;
        int nextY = positionY + directionY;
        
        if (!isValidPosition(nextX, nextY)) return count;
        if (fields[nextX][nextY].GetPiece() == null) return count;
        var tmpPlayer = fields[nextX][nextY].GetPlayer();
        if (tmpPlayer.Equals(playerName))
        {
            return checkScoreFromPoint(count + 1, nextX, nextY, directionX, directionY, playerName);
        }
        else return count;
    }

    private bool isValidPosition(int x, int y)
    {
        bool xValid = (x >= 0 && x < HorizontalSize);
        bool yValid = (y >= 0 && y < VerticalSize);
        // Debug.Log("position (" + x + ", " + y + ") is " + (xValid && yValid));
        return xValid && yValid;

    }
    
    
    /*
    private float checkVertical(int col, int row)
    {
        int column = col;
        float res = 0.0f;
        if (row > 0)
        {
            if (fields[column][row - 1].GetPlayer().Equals(currentPlayer))
            {
                res += 1f;
                return res += checkVertical(col, row - 1);
            }
        }
        return res;
    }


    private float checkHorizontal(int col, int row, bool increment)
    {
        float res = 0.0f;
        if (row > VerticalSize - 1) return res;
      
        string side;
        if (increment) side = "right";
        else side = "left";

        // Debug.Log("checking horizontal " + side + " from [" + (col - 1)+ ", " + row + "]");
      
        int column = col - 1;
        // changed below to minus two to find "off-by-1" error... ... ...
        if (increment && column < HorizontalSize - 1)
        {
            if (fields[column + 1][row].GetPlayer().Equals(currentPlayer))
            {
                res += 1f;

                return res += checkHorizontal(col + 1, row, increment);
            }
        }
        if (!increment && column > 0)
        {
            if (fields[column - 1][row].GetPlayer().Equals(currentPlayer))
            {
                res += 1f;
                return res += checkHorizontal(col - 1, row, increment);
            }
        }
        return res;
    }

    private float checkDiagonalUp(int col, int row, bool increment)
    {
        int column = col - 1;
        float res = 0.0f;
        if (increment && column < HorizontalSize - 1 && row < VerticalSize - 1)
        {
            if (fields[column + 1][row + 1].GetPlayer().Equals(currentPlayer))
            {
                res += 1f;
                return res += checkDiagonalUp(col + 1, row + 1, increment);
            }
        }
        if (!increment && column > 0 && row > 0)
        {
            if (fields[column - 1][row - 1].GetPlayer().Equals(currentPlayer))
            {
                res += 1f;
                return res += checkDiagonalUp(col - 1, row - 1, increment);
            }
        }
        return res;
    }

    private float checkDiagonalDown(int col, int row, bool increment)
    {
        int column = col - 1;
        float res = 0.0f;
        if (increment && column > 0 && row < VerticalSize - 1)
        {
            if (fields[column - 1][row + 1].GetPlayer().Equals(currentPlayer))
            {
                res += 1f;
                return res += checkDiagonalDown(col - 1, row + 1, increment);
            }

        }
        if (!increment && column < VerticalSize && row > 0)
        {
            if (fields[column + 1][row - 1].GetPlayer().Equals(currentPlayer))
            {
                res += 1f;
                return res += checkDiagonalDown(col + 1, row - 1, increment);
            }
        }
        return res;
    }
    */


    private void togglePlayer()
    {
        string tmpPlayer = otherPlayer;
        otherPlayer = currentPlayer;
        currentPlayer = tmpPlayer;
    }
    private float scoreForCenter(int col, int row)
    {
        int vertical = Mathf.Min(row, Mathf.Abs(VerticalSize - row));
        int horizontal = Mathf.Min(col, Mathf.Abs(HorizontalSize - col));
        float tempScore = vertical + horizontal;
        if (tempScore <= 0) tempScore = 0.01f;
        // return 1 - 1 / tempScore;
        return 0;
    }
    
    private int[][] createDirections()
    {
        int[][] res = new int[8][];
        int[] states = { -1, 0, 1 };
        int counter = 0;
        for (int i = 0; i < res.Length; i++)
        {
            res[i] = new int[2];
        }
        for(int i = 0; i < states.Length; i++)
        {
            
            for (int j = 0; j < states.Length; j++)
            {
                // Debug.Log("OMG!!! counter: " + counter + " i: " + i + " j: " + j + " states: " + states.Length);
                if (i == 1 && j == 1) continue;
                res[counter][0] = states[i];
                res[counter][1] = states[j];
                counter++;
            }
        }
        return res;
    }
}
