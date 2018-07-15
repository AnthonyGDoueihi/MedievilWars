using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    public BaseUnitController currentUnit;

    public bool bIsActive;
    bool isNewActive;
    float squareSize;
    Image UISelect;
    Image MoveArea;

    Square[] squares;

    // Use this for initialization
    void Start () {
        squares = FindObjectsOfType<Square>();
        squareSize = FindObjectOfType<MapGenerator>().squareSize;
        Image[] deltaImages = GetComponentsInChildren<Image>();
        if (deltaImages != null)
        {
            foreach (Image image in deltaImages)
            {
                if (image.CompareTag("MoveArea"))
                {
                    MoveArea = image;
                }
                if (image.CompareTag("UISelect"))
                {
                    UISelect = image;
                }
            }
        }

        UISelect.enabled = false;
        MoveArea.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (bIsActive)
        {
           UISelect.enabled = true;
           TestIfActive();

        }
        else
        {
            UISelect.enabled = false;
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
            foreach (Square square in squares)
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
