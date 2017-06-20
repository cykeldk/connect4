using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse 1 clicked");
            var players = GetComponentInParent<GameController>().GetComponents<PlayerInterface>();
            foreach(var player in players)
            {
                Debug.Log("player found");
                if (player.IsAi())
                {
                    Debug.Log("not a human");
                    continue;
                }
                var dude = (HumanPlayerControl)player;
                dude.setNextCol(1);
            }
            //GetComponent<HumanPlayerControl>().setNextCol(1);
            

        }
	}
}
