using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : BaseUnitController {

	// Use this for initialization
	void Start () {
        MoveRange = 4;
        AttackRange = 4;
        Attack = 3;
        Defence = 1;
        Health = 10;
}
	
	// Update is called once per frame
	void Update () {
		
	}
}
