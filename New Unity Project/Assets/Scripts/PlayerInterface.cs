using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerInterface  {

    int PlayTurn();
    bool IsAi();
    string GetName();
    string GetColor();
}
