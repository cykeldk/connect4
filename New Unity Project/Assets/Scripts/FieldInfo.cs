using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInfo : MonoBehaviour {
    private bool taken;
    public GameObject FieldPrefab;
    public GameObject BlackPrefab;
    public GameObject WhitePrefab;
    private GameObject piece;
    private string player = "none";
    
    public FieldInfo(int x, int y, GameObject go)
    {
        FieldPrefab = Instantiate(go);
        FieldPrefab.transform.position = new Vector3(x, y, 0);
    }

    public void SetPiece(GameObject go, string player)
    {
        switch (player)
        {
            case "black":
                this.player = player;
                break;
            case "white":
                this.player = player;
                break;
            default: throw (new System.Exception("shithead, it's either black or white!!!"));
        }
        this.piece = Instantiate(go);
        this.piece.transform.position = FieldPrefab.transform.position;
    }

    public GameObject GetPiece()
    {
        return this.piece;
    }

    public string GetPlayer()
    {
        return player;
    }
}
