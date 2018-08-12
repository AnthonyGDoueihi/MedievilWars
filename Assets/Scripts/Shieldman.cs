using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shieldman : BaseUnitController {

	// Use this for initialization
	void Start () {
        MoveRange = 3;
        AttackRange = 1;
        Attack = 3;
        Defence = 3;
        Health = 10;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
