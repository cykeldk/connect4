using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardControl : BoardInterface {
    public GameObject FieldPrefab;
    public GameObject white;
    public GameObject black;
    public float fieldSize;
    private FieldInfo[][] fields;
    private float horizontalOffset = 2f;
    private float verticalOffset = -4f;
    private int VerticalSize;
    private int HorizontalSize;
    private int[][] directions;

    // Use this for initialization
    public BoardControl(int horizontal, int vertical, GameObject field, GameObject p1, GameObject p2)
    {
        this.HorizontalSize = horizontal;
        this.VerticalSize = vertical;
        this.FieldPrefab = field;
        this.white = p1;
        this.black = p2;
        InitFields();
        directions = createDirections();
    }

    public void AddPiece(int col, string player)
    {
        int row = findRowInCol(col);
        addPiece(col, row, player);

    }

    private bool addPiece(int x, int y, string playerColor)
    {

        GameObject go;
        if (playerColor.Equals("black")) go = black;
        else if (playerColor.Equals("white")) go = white;
        else throw new System.Exception("Fucktard, you failed to specify or spell which player it is!!");
        if (!IsValidPosition(x,y)) return false;
        if (fields[x][y].GetPiece() == null)
        {
            fields[x][y].SetPiece(go, playerColor);
            return true;
        }
        return addPiece(x, y + 1, playerColor);
    }

    public GameObject GetPieceAt(int x, int y)
    {
        return fields[x][y].GetPiece();
    }

    public string GetPlayerAt(int x, int y)
    {
        if (IsValidPosition(x,y)) return fields[x][y].GetPlayer();
        return null;
    }

    public int findRowInCol(int col)
    {

        int counter = 0;
        while (counter < VerticalSize && fields[col][counter].GetPiece() != null)
        {
            counter++;
        }
        return counter;
    }
    
    public float checkCol(int col, string playerColor)
    {

        if (GetPieceAt(col, VerticalSize - 1) != null)
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

            var scoreDirection1 = checkScoreFromPoint(1, col, row, direction1[0], direction1[1], playerColor);
            var scoreDirection2 = checkScoreFromPoint(1, col, row, direction2[0], direction2[1], playerColor);

            debugString += i + "(" + direction1[0] + ", " + direction1[1] + ")  ::: score = " + scoreDirection1 + "\n";
            debugString += i + "(" + direction2[0] + ", " + direction2[1] + ")  ::: score = " + scoreDirection2 + "\n";
            debugString += "Sum : " + (scoreDirection1 + scoreDirection2) + "\n";

            var tempScore = (scoreDirection1 + scoreDirection2 - 1); // minus one because checkScore from point counts the field it starts on

            if (tempScore > tmp)
            {
                tmp = tempScore;
            }
        }
        //Debug.Log(debugString);
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




    public int checkScoreFromPoint(int count, int positionX, int positionY, int directionX, int directionY, string playerName)
    {
        int nextX = positionX + directionX;
        int nextY = positionY + directionY;
        if (!IsValidPosition(nextX, nextY)) return count;
        if (GetPieceAt(nextX, nextY) == null) return count;
        var tmpPlayer = GetPlayerAt(nextX, nextY);
        if (tmpPlayer.Equals(playerName))
        {
            return checkScoreFromPoint(count + 1, nextX, nextY, directionX, directionY, playerName);
        }
        else return count;
    }


    public bool IsValidPosition(int x, int y)
    {
        bool xValid = (x >= 0 && x < HorizontalSize);
        bool yValid = (y >= 0 && y < VerticalSize);
        // Debug.Log("position (" + x + ", " + y + ") is " + (xValid && yValid));
        return xValid && yValid;

    }

    public int GetVerticalSize()
    {
        return VerticalSize;
    }

    public int GetHorizontalSize()
    {
        return HorizontalSize;
    }

    public int[][] GetDirections()
    {
        return directions;
    }

    public void InitFields()
    {
        fields = new FieldInfo[HorizontalSize][];
        for (int i = 0; i < HorizontalSize; i++)
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

    private int[][] createDirections()
    {
        int[][] res = new int[8][];
        int[] states = { -1, 0, 1 };
        int counter = 0;
        for (int i = 0; i < res.Length; i++)
        {
            res[i] = new int[2];
        }
        for (int i = 0; i < states.Length; i++)
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
