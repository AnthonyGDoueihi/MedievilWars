using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : BaseUnitController {

	// Use this for initialization
	void Start () {
        bIsSummoner = true;
        MoveRange = 2;
        AttackRange = 0;
        Attack = 0;
        Defence = 2;
        Health = 20;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
