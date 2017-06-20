using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BoardInterface {

    void AddPiece(int col, string player);
    GameObject GetPieceAt(int x, int y);
    string GetPlayerAt(int x, int y);
    int findRowInCol(int col);
    bool IsValidPosition(int x, int y);
    int GetVerticalSize();
    int GetHorizontalSize();
    int[][] GetDirections();
    float checkCol(int col, string playerName);
    int checkScoreFromPoint(int count, int positionX, int positionY, int directionX, int directionY, string playerName);
}
