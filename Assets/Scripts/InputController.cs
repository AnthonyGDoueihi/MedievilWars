using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    bool isController;
    bool isMouse;
    float boundryPercent = 0.1f;
    CameraController cam;
    public Canvas UIActiveSquare;
    public SquareTile[,] squareCoord;
    int[] currentActiveSquare = new int[2];
    float moveSquareTime = 0f;
    public float reloadTime = 0.1f;

    bool bUnitMenu;
    bool bIsChooseMove;
    bool bIsChooseAttack;
    bool bIsSummoner;

    List<SquareTile> listFloodFillResult = new List<SquareTile>();
    UnitMenu unitMenu;
    public GameObject uiOverlay;
    public Canvas moveArea;
    public Canvas attackArea;

    // Use this for initialization
    void Start() {
        cam = Camera.main.GetComponent<CameraController>();
        isMouse = true;
        UIActiveSquare = Instantiate(UIActiveSquare);
        unitMenu = FindObjectOfType<UnitMenu>();
    }

    // Update is called once per frame
    void Update() {
        
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
        Select();
    }

    void CameraInputs()
    {
        if (isMouse)
        {
            if (Input.mousePosition.x > Screen.width - (Screen.width * boundryPercent))
            {
                cam.bMoveRight = true;
            }
            else
            {
                cam.bMoveRight = false;
            }

            if (Input.mousePosition.x < (Screen.width * boundryPercent))
            {
                cam.bMoveLeft = true;
            }
            else
            {
                cam.bMoveLeft = false;
            }

            if (Input.mousePosition.y > Screen.height - (Screen.height * boundryPercent))
            {
                cam.bMoveUp = true;
            }
            else
            {
                cam.bMoveUp = false;
            }

            if (Input.mousePosition.y < (Screen.height * boundryPercent))
            {
                cam.bMoveDown = true;
            }
            else
            {
                cam.bMoveDown = false;
            }
        }

        if (isController)
        {

            if (Input.GetAxis("CamRight") < 0)
            {
                cam.bMoveRight = true;
                cam.bMoveLeft = false;

            }
            else if (Input.GetAxis("CamRight") > 0)
            {
                cam.bMoveRight = false;
                cam.bMoveLeft = true;

            }
            else
            {
                cam.bMoveRight = false;
                cam.bMoveLeft = false;

            }

            if (Input.GetAxis("CamUp") < 0)
            {
                cam.bMoveUp = false;
                cam.bMoveDown = true;

            }
            else if (Input.GetAxis("CamUp") > 0)
            {
                cam.bMoveUp = true;
                cam.bMoveDown = false;

            }
            else
            {
                cam.bMoveUp = false;
                cam.bMoveDown = false;

            }
        }
    }

    void ActiveSquare()
    {
        if (UIActiveSquare != null)
        {
            UIActiveSquare.GetComponent<RectTransform>().position = squareCoord[currentActiveSquare[0], currentActiveSquare[1]].gameObject.transform.position;
        }

        if (isMouse)
        {
            if (!bUnitMenu)            
            {
                int layerMask = 1 << 9;
                layerMask = ~layerMask;
                RaycastHit2D rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, Mathf.Infinity, layerMask);
                if (rayHit.collider != null)
                {
                    SquareTile squareTile = rayHit.collider.gameObject.GetComponent<SquareTile>();
                    squareTile.bIsActive = true;
                    currentActiveSquare = squareTile.coord;
                }
            }
        }

        if (isController)
        {
            if (!bUnitMenu)
            {
                bool bSquareUp = false;
                bool bSquareDown = false;
                bool bSquareRight = false;
                bool bSquareLeft = false;

                moveSquareTime -= Time.deltaTime;

                if (Input.GetAxis("Vertical") < -0.5f)
                {
                    bSquareUp = false;
                    bSquareDown = true;
                }
                else if (Input.GetAxis("Vertical") > 0.5f)
                {
                    bSquareUp = true;
                    bSquareDown = false;
                }
                else
                {
                    bSquareUp = false;
                    bSquareDown = false;
                }

                if (Input.GetAxis("Horizontal") < -0.5f)
                {
                    bSquareRight = false;
                    bSquareLeft = true;
                }
                else if (Input.GetAxis("Horizontal") > 0.5f)
                {
                    bSquareRight = true;
                    bSquareLeft = false;
                }
                else
                {
                    bSquareRight = false;
                    bSquareLeft = false;
                }

                if (moveSquareTime <= 0)
                {
                    int x = currentActiveSquare[0];
                    int y = currentActiveSquare[1];
                    if (bSquareUp)
                    {
                        if (y + 1 < squareCoord.GetLength(1))
                        {
                            moveSquareTime = reloadTime;
                            squareCoord[x, y].bIsActive = false;
                            squareCoord[x, y + 1].bIsActive = true;
                            currentActiveSquare = squareCoord[x, y + 1].coord;
                            return;
                        }
                    }
                    if (bSquareDown)
                    {
                        if (y - 1 > 0)
                        {
                            moveSquareTime = reloadTime;
                            squareCoord[x, y].bIsActive = false;
                            squareCoord[x, y - 1].bIsActive = true;
                            currentActiveSquare = squareCoord[x, y - 1].coord;
                            return;
                        }
                    }
                    if (bSquareRight)
                    {
                        if (x + 1 < squareCoord.GetLength(0))
                        {
                            moveSquareTime = reloadTime;
                            squareCoord[x, y].bIsActive = false;
                            squareCoord[x + 1, y].bIsActive = true;
                            currentActiveSquare = squareCoord[x + 1, y].coord;
                            return;
                        }
                    }
                    if (bSquareLeft)
                    {
                        if (x - 1 > 0)
                        {
                            moveSquareTime = reloadTime;
                            squareCoord[x, y].bIsActive = false;
                            squareCoord[x - 1, y].bIsActive = true;
                            currentActiveSquare = squareCoord[x - 1, y].coord;
                            return;
                        }
                    }
                }
            }
        }
    }

    void Select()
    {
        if (Input.GetButtonDown("Forward"))
        {
            if (!bUnitMenu)
            {
                if (!bIsChooseMove && !bIsChooseAttack)
                {
                    SquareTile current = squareCoord[currentActiveSquare[0], currentActiveSquare[1]];
                    if (current.currentUnit != null)
                    {
                        bIsSummoner = current.currentUnit.bIsSummoner;
                        unitMenu.Activate(bIsSummoner);
                        bUnitMenu = true;
                    }
                }
                else
                {
                    SquareTile current = squareCoord[currentActiveSquare[0], currentActiveSquare[1]];
                    if (listFloodFillResult.Contains(current))
                    {
                        if (bIsChooseAttack)
                        {
                            //TODO attack
                            print("attack this");
                        }
                        else if (bIsChooseMove)
                        {
                            //TODO move
                            print("move here");
                        }

                    }
                }
            }

        }

        if (Input.GetButtonDown("Back"))
        {
            Back();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (bUnitMenu || bIsChooseAttack || bIsChooseMove)
            {
                Back();
            }
            else
            {
                
            }
        }
    }

    void Back()
    {
        if (bUnitMenu && !bIsChooseAttack && !bIsChooseMove)
        {
            unitMenu.Deactivate();
            bIsSummoner = false;
            bUnitMenu = false;
        }

        if (bIsChooseAttack || bIsChooseMove)
        {
            bUnitMenu = true;

            unitMenu.Activate(bIsSummoner);
            foreach (Transform child in uiOverlay.transform)
            {
                Destroy(child.gameObject);
            }
            bIsChooseAttack = false;
            bIsChooseMove = false;
        }
    }

    public void AttackFind()
    {
        if (listFloodFillResult != null)
        {
            listFloodFillResult.Clear();
        }

        bIsChooseAttack = true;
        int range = squareCoord[currentActiveSquare[0], currentActiveSquare[1]].GetComponent<BaseUnitController>().AttackRange;
        FloodFill(currentActiveSquare[0], currentActiveSquare[1], range);

        foreach (SquareTile x in listFloodFillResult)
        {
            Instantiate(attackArea, x.transform.position, Quaternion.identity, uiOverlay.transform);
        }
        bUnitMenu = false;

    }

    public void MoveFind()
    {
        if (listFloodFillResult != null)
        {
            listFloodFillResult.Clear();
        }

        bIsChooseMove = true;
        int range = squareCoord[currentActiveSquare[0], currentActiveSquare[1]].currentUnit.MoveRange;

        FloodFill(currentActiveSquare[0], currentActiveSquare[1], range);

        foreach (SquareTile x in listFloodFillResult)
        {
            Instantiate(moveArea, x.transform.position, Quaternion.identity, uiOverlay.transform);
        }
        bUnitMenu = false;
    }


    void FloodFill(int xCoord, int yCoord, int movesLeft)
    {

        int thisMoveLeft = movesLeft - 1;
        
        if (!listFloodFillResult.Contains(squareCoord[xCoord - 1, yCoord]))
        {
            if (bIsChooseMove)
            {
                if (squareCoord[xCoord -1, yCoord].gameObject.layer == 8 && squareCoord[xCoord - 1, yCoord].currentUnit == null)
                {
                    listFloodFillResult.Add(squareCoord[xCoord - 1, yCoord]);
                }
                
            }

            if (bIsChooseAttack)
            {
                if (squareCoord[xCoord -1, yCoord].currentUnit != null)
                {
                    if (squareCoord[xCoord, yCoord].currentUnit.bIsPlayer != squareCoord[xCoord -1, yCoord].currentUnit.bIsPlayer)
                    {
                        listFloodFillResult.Add(squareCoord[xCoord - 1, yCoord]);
                    }
                }                               
            }
            
        }

        if (!listFloodFillResult.Contains(squareCoord[xCoord + 1, yCoord]))
        {
            if (bIsChooseMove)
            {
                if (squareCoord[xCoord + 1, yCoord].gameObject.layer == 8 && squareCoord[xCoord + 1, yCoord].currentUnit == null)
                {
                    listFloodFillResult.Add(squareCoord[xCoord + 1, yCoord]);
                }
            }

            if (bIsChooseAttack)
            {
                if (squareCoord[xCoord + 1, yCoord].currentUnit != null)
                {
                    if (squareCoord[xCoord, yCoord].currentUnit.bIsPlayer != squareCoord[xCoord + 1, yCoord].currentUnit.bIsPlayer)
                    {
                        listFloodFillResult.Add(squareCoord[xCoord + 1, yCoord]);
                    }
                }
            }

        }

        if (!listFloodFillResult.Contains(squareCoord[xCoord, yCoord - 1]))
        {
            if (bIsChooseMove)
            {
                if (squareCoord[xCoord, yCoord - 1].gameObject.layer == 8 && squareCoord[xCoord, yCoord - 1].currentUnit == null)
                {
                    listFloodFillResult.Add(squareCoord[xCoord, yCoord - 1]);
                }
            }

            if (bIsChooseAttack)
            {
                if (squareCoord[xCoord, yCoord - 1].currentUnit != null)
                {
                    if (squareCoord[xCoord, yCoord].currentUnit.bIsPlayer != squareCoord[xCoord, yCoord - 1].currentUnit.bIsPlayer)
                    {
                        listFloodFillResult.Add(squareCoord[xCoord, yCoord - 1]);
                    }
                }
            }

        }

        if (!listFloodFillResult.Contains(squareCoord[xCoord, yCoord + 1]))
        {
            if (bIsChooseMove)
            {
                if (squareCoord[xCoord, yCoord + 1].gameObject.layer == 8 && squareCoord[xCoord, yCoord + 1].currentUnit == null)
                {
                    listFloodFillResult.Add(squareCoord[xCoord, yCoord + 1]);
                }
            }

            if (bIsChooseAttack)
            {
                if (squareCoord[xCoord, yCoord + 1].currentUnit != null)
                {
                    if (squareCoord[xCoord, yCoord].currentUnit.bIsPlayer != squareCoord[xCoord, yCoord + 1].currentUnit.bIsPlayer)
                    {
                        listFloodFillResult.Add(squareCoord[xCoord, yCoord + 1]);
                    }
                }
            }

        }


        if (thisMoveLeft > 0)
        {
            FloodFill(xCoord - 1, yCoord, thisMoveLeft);
            FloodFill(xCoord + 1, yCoord, thisMoveLeft);
            FloodFill(xCoord, yCoord - 1, thisMoveLeft);
            FloodFill(xCoord, yCoord + 1, thisMoveLeft);
        }
    }

}
