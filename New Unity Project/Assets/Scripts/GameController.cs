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
    
    // variables used for testing
    public int addPieces;
    public bool colsTested = false;
    public int[] colsToTest = {5};
    public string currentPlayer = "black";


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

        int input = 0;

        if (currentPlayer.Equals("white"))
        {
            int lastCol = PlayTurn();
            
            if (checkWin(lastCol))
            {
                Debug.Log("------------------------------------------------" + currentPlayer + " wins");
                
            }
            else
            {
                currentPlayer = "black";
            }
            
        }


        else
        {
            int lastCol = PlayTurn();
            if (checkWin(lastCol))
            {
                Debug.Log("current player wins");
            }
            currentPlayer = "white";
        }
	}

    public int PlayTurn()
    {
        List<int> possibleColsToPlay = new List<int>();
        float tempScore = 0f;
        
        for (int i = 0; i < HorizontalSize; i++)
        {
            var score = checkCol(i);

            if (score == tempScore)
            {
                Debug.Log("adding " + i + " to possible cols");
                possibleColsToPlay.Add(i);
            }
            else if (score > tempScore)
            {
                Debug.Log("score " + score + " is new highscore at " + i);
                possibleColsToPlay.Clear();
                possibleColsToPlay.Add(i);
                tempScore = checkCol(i);
            }
            //Debug.Log("column " + i + " has a value of " + tempScore);
        }
        int indexToPlay = sysrand.Next(possibleColsToPlay.Count);
        Debug.Log("amount: " + possibleColsToPlay.Count + " index chosen: " + indexToPlay);
        int colToPlay = possibleColsToPlay[indexToPlay];
        AddPiece(colToPlay, currentPlayer);
        
        Debug.Log(currentPlayer + " is playing column " + colToPlay);
        return colToPlay;
    }

    public void AddPiece(int col, string player)
    {
        int row = findRowInCol(col);
        if (addPiece(col, row, player))
        {
            // Debug.Log("piece added at col: [" + col + "] row: [" + row + "]");
            
        }
        else
        {
            // Debug.Log(col + " is full");
        }
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

    private bool checkWin(int col)
    {
        Debug.Log("checking for winner on col" + col);
        bool win = false;
        int row = findRowInCol(col);
        if (checkHorizontal(col, row, true) > scoreToWin || checkHorizontal(col, row, false) > scoreToWin) return true;

        return win;
    }

    private float checkCol(int col)
    {
        
        if (fields[col][VerticalSize - 1].GetPiece()!=null)
        {
            Debug.Log("column " + col + " is full");
            return 0;
        }
        int tmp = 0;
        foreach(var direction in directions)
        {
            tmp += checkPoints(0, col, findRowInCol(col), direction[0], direction[1]);
        }
        Debug.Log("score for col " + col + ": " + tmp);
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

    
    private int checkPoints(int count, int positionX, int positionY, int directionX, int directionY)
    {
        
        int nextX = positionX + directionX;
        int nextY = positionY + directionY;
        if (!isValidPosition(nextX, nextY)) return 0;
        var tmpPlayer = fields[nextX][nextY].GetPlayer();
        if (tmpPlayer.Equals(currentPlayer))
        {
            return 1 + checkPoints(count + 1, nextX, nextY, directionX, directionY);
        }
        else return 0;
    }

    private bool isValidPosition(int x, int y)
    {
        bool xValid = (x >= 0 && x < HorizontalSize);
        bool yValid = (y >= 0 && y < VerticalSize);
        Debug.Log("position (" + x + ", " + y + ") is " + (xValid && yValid));
        return xValid && yValid;

    }
    
    
    
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
