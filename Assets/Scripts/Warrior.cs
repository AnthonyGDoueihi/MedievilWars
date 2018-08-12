using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : BaseUnitController {

	// Use this for initialization
	void Start () {
        MoveRange = 5;
        AttackRange = 1;
        Attack = 4;
        Defence = 1;
        Health = 10;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
