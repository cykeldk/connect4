using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerInterface  {

    bool PlayTurn();
    bool IsAi();
    string GetName();
    string GetColor();
}
