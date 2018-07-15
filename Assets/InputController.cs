using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    bool isController;
    bool isMouse;
    float boundryPercent = 0.1f;
    CameraController camera;

    // Use this for initialization
    void Start () {
        camera = Camera.main.GetComponent<CameraController>();
        isMouse = true;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            isMouse = true;
            isController = false;
        }

        if (Input.GetAxis("CamUp") != 0 || Input.GetAxis("CamRight") != 0)
        {
            isMouse = false;
            isController = true;
        }

        CameraInputs();
        ActiveSquare();

    }

    void CameraInputs()
    {
        if (isMouse)
        {
            if (Input.mousePosition.x > Screen.width - (Screen.width * boundryPercent))
            {
                camera.bMoveRight = true;
            }
            else
            {
                camera.bMoveRight = false;
            }

            if (Input.mousePosition.x < (Screen.width * boundryPercent))
            {
                camera.bMoveLeft = true;
            }
            else
            {
                camera.bMoveLeft = false;
            }

            if (Input.mousePosition.y > Screen.height - (Screen.height * boundryPercent))
            {
                camera.bMoveUp = true;
            }
            else
            {
                camera.bMoveUp = false;
            }

            if (Input.mousePosition.y < (Screen.height * boundryPercent))
            {
                camera.bMoveDown = true;
            }
            else
            {
                camera.bMoveDown = false;
            }
        }

        if (isController)
        {

            if(Input.GetAxis("CamRight") < 0)
            {
                camera.bMoveRight = true;
                camera.bMoveLeft = false;

            }
            else if(Input.GetAxis("CamRight") > 0)
            {
                camera.bMoveRight = false;
                camera.bMoveLeft = true;

            }
            else
            {
                camera.bMoveRight = false;
                camera.bMoveLeft = false;

            }

            if (Input.GetAxis("CamUp") < 0)
            {
                camera.bMoveUp = false;
                camera.bMoveDown = true;

            }
            else if (Input.GetAxis("CamUp") > 0)
            {
                camera.bMoveUp = true;
                camera.bMoveDown = false;

            }
            else
            {
                camera.bMoveUp = false;
                camera.bMoveDown = false;

            }
        }
    }

    void ActiveSquare()
    {
        if (isMouse)
        {
            RaycastHit rayHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayHit))
            {
                rayHit.collider.gameObject.GetComponent<Square>().bIsActive = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Gizmos.DrawRay(ray);

    }

}
