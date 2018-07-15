using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;
    public float squareSize = 0.32f;

    [Range(40, 50)]
    public int randomFillPercent;

    int[,] map;


    private void Start()
    {
        GenerateMap();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           // GenerateMap();
        }
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }

        ProcessMap();

        WaterGenerator waterGen = GetComponent<WaterGenerator>();
        waterGen.GenerateMap(map, squareSize);

        ArmyGenerator armyGen = GetComponent<ArmyGenerator>();
        armyGen.GenerateArmy(width, height, squareSize);
    }

    void ProcessMap()
    {
        List<List<Coord>> islandRegions = GetRegions(0);
        int islandThreshholdSize = 50;
        List<Island> survivingIsland = new List<Island>();

        foreach (List<Coord> islandRegion in islandRegions)
        {
            if (islandRegion.Count < islandThreshholdSize)
            {
                foreach (Coord tile in islandRegion)
                {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
            else
            {
                survivingIsland.Add(new Island(islandRegion, map));
            }
        }

        survivingIsland.Sort();
        survivingIsland[0].isMainIsland = true;
        survivingIsland[0].isConnectedToMain = true;

        ConnectIslands(survivingIsland);
    }

    void ConnectIslands(List<Island> allIslands, bool forceConnection = false)
    {
        List<Island> islandListA = new List<Island>();
        List<Island> islandListB = new List<Island>();

        if (forceConnection)
        {
            foreach (Island isle in allIslands)
            {
                if (isle.isConnectedToMain)
                {
                    islandListB.Add(isle);
                }
                else
                {
                    islandListA.Add(isle);
                }
            }
        }
        else
        {
            islandListA = allIslands;
            islandListB = allIslands;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Island bestIslandA = new Island();
        Island bestIslandB = new Island();
        bool possibleConnectionFound = false;

        foreach (Island islandA in islandListA)
        {
            if (!forceConnection)
            {
                possibleConnectionFound = false;
                if (islandA.connectedIsland.Count > 0)
                {
                    continue;
                }
            }
            foreach (Island islandB in islandListB)
            {
                if (islandA == islandB || islandA.IsConnected(islandB))
                {
                    continue;
                }

                for (int tileIndexA = 0; tileIndexA < islandA.coastTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < islandB.coastTiles.Count; tileIndexB++)
                    {
                        Coord tileA = islandA.coastTiles[tileIndexA];
                        Coord tileB = islandB.coastTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestIslandA = islandA;
                            bestIslandB = islandB;
                        }
                    }
                }
            }

            if (possibleConnectionFound && !forceConnection)
            {
                CreatePassage(bestIslandA, bestIslandB, bestTileA, bestTileB);
            }
        }

        if (possibleConnectionFound && forceConnection)
        {
            CreatePassage(bestIslandA, bestIslandB, bestTileA, bestTileB);
            ConnectIslands(allIslands, true);
        }
        if (!forceConnection)
        {
            ConnectIslands(allIslands, true);
        }
    }

    List <Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();

        int x = from.tileX;
        int y = from.tileY;
        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;

        bool inverted = false;
        int step = (int) Mathf.Sign(dx);
        int gradientStep = (int) Mathf.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);
            step = (int)Mathf.Sign(dy);
            gradientStep = (int)Mathf.Sign(dx);
        }

        int gradientAccumilation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));
            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumilation += shortest;
            if (gradientAccumilation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumilation -= longest;
            }
        }

        return line;
    }

    void CreatePassage(Island islandA, Island islandB, Coord tileA, Coord tileB)
    {
        Island.ConnectIslands(islandA, islandB);

        List<Coord> line = GetLine(tileA, tileB);
        foreach (Coord c in line)
        {
            DrawCircle(c, 1);
        }
    }

    void DrawCircle(Coord c, int r)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x*x + y*y <= r * r)
                {
                    int realX = c.tileX + x;
                    int realY = c.tileY + y;
                    if(realX>= 0 && realX< width && realY>= 0 && realY< height)
                    {
                        map[realX,realY] = 0;
                    }
                }
            }
        }
    }

    Vector3 CoordToWorldPoints(Coord tile)
    {
        return (new Vector3(tile.tileX * squareSize, tile.tileY * squareSize, 2));
    }

    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (x >= 0 && x < width && y >= 0 && y < height && (y == tile.tileY || x == tile.tileX))
                    {
                        if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = System.DateTime.Now.Ticks.ToString(); ;
        }
        UnityEngine.Random.InitState(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x <= 0 || x >= width - 1 || y <= 0 || y >= height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (UnityEngine.Random.value < (randomFillPercent / 100f)) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                int neighbourWaterTiles = GetSurroundingWaterCount(x, y);

                if (neighbourWaterTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWaterTiles < 4)
                    map[x, y] = 0;

            }
        }
    }


    int GetSurroundingWaterCount(int gridX, int gridY)
    {
        int waterCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        waterCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    waterCount++;
                }
            }
        }
        return waterCount;
    }


    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }


    class Island : IComparable<Island>
    {
        public List<Coord> tiles;
        public List<Coord> coastTiles;
        public List<Island> connectedIsland;
        public int islandSize;
        public bool isMainIsland;
        public bool isConnectedToMain;

        public Island()
        {

        }

        public Island(List<Coord> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            islandSize = tiles.Count;
            connectedIsland = new List<Island>();

            coastTiles = new List<Coord>();
            foreach (Coord tile in tiles)
            {
                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                    {
                        if (x == tile.tileX || y == tile.tileY)
                        {
                            if (map[x, y] == 1)
                            {
                                coastTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainIsland()
        {
            if (!isConnectedToMain)
            {
                isConnectedToMain = true;
                foreach (Island isle in connectedIsland)
                {
                    isle.SetAccessibleFromMainIsland();
                }
            }
        }

        public static void ConnectIslands(Island islandA, Island islandB)
        {
            if (islandA.isConnectedToMain)
            {
                islandB.SetAccessibleFromMainIsland();
            }
            else if (islandB.isConnectedToMain)
            {
                islandA.SetAccessibleFromMainIsland();
            }
            islandA.connectedIsland.Add(islandB);
            islandB.connectedIsland.Add(islandA);
        }

        public bool IsConnected(Island otherIsland)
        {
            return (connectedIsland.Contains(otherIsland));
        }

        public int CompareTo(Island otherRoom)
        {
            return otherRoom.islandSize.CompareTo(islandSize);
        }

    }

    /*
          private void OnDrawGizmos()
       {
           if (map != null)
           {
               for (int x = 0; x < width; x++)
               {
                   for (int y = 0; y < height; y++)
                   {
                       Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                       Vector2 pos = new Vector2(x + .5f * squareSize, y + .5f* squareSize);
                       Gizmos.DrawCube(pos, Vector3.one * 0.4f);
                   }
               }
           }
       }
       */
}
