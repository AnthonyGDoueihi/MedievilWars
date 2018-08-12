using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareTile : MonoBehaviour {

    public BaseUnitController currentUnit;

    public bool bIsActive;
    bool isNewActive = true;
    float squareSize;

    SquareTile[] squares;
    public int[] coord = new int[2];


    // Use this for initialization
    void Start () {
        squares = FindObjectsOfType<SquareTile>();
        squareSize = FindObjectOfType<MapGenerator>().squareSize;        
	}

	// Update is called once per frame
	void Update () {
		if (bIsActive)
        {
           TestIfActive();
            print(currentUnit);
        }
        else
        {

        }
        
	}

    void TestIfActive()
    {
        if (isNewActive)
        {
            isNewActive = false;
            return;
        }
        else
        {
            foreach (SquareTile square in squares)
            {
                if (square != this)
                {
                    if (square.bIsActive)
                    {
                        bIsActive = false;
                        isNewActive = true;
                    }
                }
            }
        }
        
    }

}
