using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyGenerator : MonoBehaviour {

    WaterGenerator waterGen;
    MapGenerator mapGen;
    public BaseUnitController U_playerSummoner;
    public BaseUnitController U_playerWarrior;
    public BaseUnitController U_playerShieldman;
    public BaseUnitController U_playerCaster;
    public BaseUnitController U_playerArcher;
    public GameObject playerFolder;
    public BaseUnitController U_enemySummoner;
    public BaseUnitController U_enemyWarrior;
    public BaseUnitController U_enemyShieldman;
    public BaseUnitController U_enemyCaster;
    public BaseUnitController U_enemyArcher;
    public GameObject enemyFolder;

    List<GameObject> playerSpawnPoints;
    List<GameObject> enemySpawnPoints;

    float squareSize;

    public void GenerateArmy(int width, int height, float size)
    {
        squareSize = size;
        float mapWidth = (width -1) * squareSize;
        float mapHeight = (height -1) * squareSize;
        float spawnWidth = mapWidth / squareSize / 10;
        float spawnHeight = mapHeight / squareSize / 10 ;

        int unitToSpawn = (int) (spawnWidth * spawnHeight) / 4;

        foreach (Transform child in playerFolder.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in enemyFolder.transform)
        {
            Destroy(child.gameObject);
        }

        int playerGroundCount = 0;
        int playerTimesThrough = 0;
        while (playerGroundCount <= unitToSpawn)
        {
            GetPlayerBox(mapWidth, mapHeight, spawnWidth, spawnHeight, playerTimesThrough, out playerGroundCount);
            playerTimesThrough ++;
        }

        int enemyGroundCount = 0;
        int enemyTimesThrough = 0;
        while (enemyGroundCount <= unitToSpawn)
        {
            GetEnemyBox(mapWidth, mapHeight, spawnWidth, spawnHeight, enemyTimesThrough, out enemyGroundCount);
            enemyTimesThrough++;
        }

        SpawnUnits(unitToSpawn);

    }

    void GetPlayerBox(float mapWidth, float mapHeight, float spawnWidth, float spawnHeight, int timesThrough, out int playerGroundCount)
    {
        playerGroundCount = 0;
        if (playerSpawnPoints != null)
        {
            playerSpawnPoints.Clear();
        }
        RaycastHit2D[] playerHit = Physics2D.BoxCastAll(
            new Vector2(0 + (timesThrough * squareSize), (mapHeight - (timesThrough * squareSize) - spawnHeight)),
            new Vector2(spawnWidth, spawnHeight), 0, Vector2.zero);
        GameObject[] deltaArray = new GameObject[playerHit.Length];
        foreach (RaycastHit2D hit in playerHit)
        {
            if (hit.collider.gameObject.layer == 8)
            {
                deltaArray[playerGroundCount] = hit.collider.gameObject;
                playerGroundCount++;
            }
        }

        playerSpawnPoints = new List<GameObject>(playerGroundCount);
        for (int i = 0; i < playerGroundCount; i++)
        {
            playerSpawnPoints.Add(deltaArray[i]);
        }
    }

    void GetEnemyBox(float mapWidth, float mapHeight, float spawnWidth, float spawnHeight, int timesThrough, out int enemyGroundCount)
    {
        enemyGroundCount = 0;
        if (enemySpawnPoints != null)
        {
            enemySpawnPoints.Clear();
        }
        RaycastHit2D[] enemyHit = Physics2D.BoxCastAll(
            new Vector2(mapWidth - (timesThrough * squareSize) - spawnWidth, 0 + (timesThrough*squareSize)),
            new Vector2(spawnWidth, spawnHeight), 0, Vector2.zero);
        GameObject[] deltaArray = new GameObject[enemyHit.Length];
        foreach (RaycastHit2D hit in enemyHit)
        {
            if (hit.collider.gameObject.layer == 8)
            {
                deltaArray[enemyGroundCount] = hit.collider.gameObject;
                enemyGroundCount++;
            }
        }

        enemySpawnPoints = new List<GameObject>(enemyGroundCount);
        for (int i = 0; i < enemyGroundCount; i++)
        {
            enemySpawnPoints.Add(deltaArray[i]);
        }
    }

    void SpawnUnits(int unitToSpawn)
    {
        bool isPlayer = true;
        bool isEnemy = false;
        SpawnUnit(isPlayer, U_playerSummoner, 1);
        SpawnUnit(isEnemy, U_enemySummoner, 1);
        unitToSpawn--;

        int warriorsToSpawn = unitToSpawn / 3;
        SpawnUnit(isPlayer, U_playerWarrior, warriorsToSpawn);
        SpawnUnit(isEnemy, U_enemyWarrior, warriorsToSpawn);
        unitToSpawn -= warriorsToSpawn;

        int archersToSpawn = unitToSpawn / 3;
        SpawnUnit(isPlayer, U_playerArcher, archersToSpawn);
        SpawnUnit(isEnemy, U_enemyArcher, archersToSpawn);
        unitToSpawn -= archersToSpawn;

        int shieldToSpawn = unitToSpawn / 2;
        SpawnUnit(isPlayer, U_playerShieldman, shieldToSpawn);
        SpawnUnit(isEnemy, U_enemyShieldman, shieldToSpawn);
        unitToSpawn -= shieldToSpawn;

        int casterToSpawn = unitToSpawn;
        SpawnUnit(isPlayer, U_playerCaster, casterToSpawn);
        SpawnUnit(isEnemy, U_enemyCaster, casterToSpawn);
    }


    void SpawnUnit(bool isPlayer, BaseUnitController toSpawn, int numberToSpawn)
    {     
        if (isPlayer)
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                int position = Random.Range(0, playerSpawnPoints.Count - 1);
                Instantiate(toSpawn, playerSpawnPoints[position].transform.position, Quaternion.identity, playerFolder.transform);
                playerSpawnPoints.RemoveAt(position);
            }
        }
        else
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                int position = Random.Range(0, enemySpawnPoints.Count - 1);
                Instantiate(toSpawn, enemySpawnPoints[position].transform.position, Quaternion.identity, enemyFolder.transform);
                enemySpawnPoints.RemoveAt(position);
            }
        }
        
    }
}
