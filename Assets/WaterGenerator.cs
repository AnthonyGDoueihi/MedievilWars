using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour {

    public SquareGrid squareGrid;
    public GameObject[] background = new GameObject[16];
    public Canvas backgroundCanvas;
    
    public void GenerateMap(int[,] map, float squareSize)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        squareGrid = new SquareGrid(map, squareSize);

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                Square currentSquare = squareGrid.squares[x, y];
                GameObject tile = Instantiate(background[currentSquare.configuration], currentSquare.bottomLeft.position, Quaternion.identity);
                Instantiate(backgroundCanvas, tile.transform);
                tile.transform.parent = this.transform;
            }
        }


    }

   
    public class SquareGrid
    {
        public Square[,] squares;
        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector2 pos = new Vector2(x * squareSize + squareSize / 2, y * squareSize + squareSize / 2);

                    controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }
        }
    }
    

    public class Square
    {
        public ControlNode topLeft, topRight, bottomRight, bottomLeft;
        public Node centreTop, centreRight, centreBottom, centreLeft;
        public int configuration;

        public Square (ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
        {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            centreTop = topLeft.right;
            centreRight = bottomRight.up;
            centreBottom = bottomLeft.right;
            centreLeft = bottomLeft.up;

            if (topLeft.active)
                configuration += 8;
            if (topRight.active)
                configuration += 4;
            if (bottomRight.active)
                configuration += 2;
            if (bottomLeft.active)
                configuration += 1;
        }
    }

    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _pos)
        {
            position = _pos;
        }
    }

    public class ControlNode : Node
    {
        public bool active;
        public Node up, right;

        public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos)
        {
            active = _active;
            up = new Node(position + Vector3.up * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(squareGrid.squares[0, 0].topLeft.position, Vector3.one * .4f);

        Gizmos.color = Color.red;
        //Gizmos.DrawCube(squareGrid.squares[squareGrid.squares.GetLength(0) - 1, squareGrid.squares.GetLength(1) - 1].topLeft.position, Vector3.one * .4f);
        Gizmos.DrawCube(squareGrid.squares[98, 98].topLeft.position, Vector3.one * .4f);

    }
    */
}
