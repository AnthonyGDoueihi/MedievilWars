using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyGenerator : MonoBehaviour {

    WaterGenerator waterGen;
    MapGenerator mapGen;
    public Summoner U_playerSummoner;
    public Warrior U_playerWarrior;
    public Shieldman U_playerShieldman;
    public Caster U_playerCaster;
    public Archer U_playerArcher;
    public GameObject playerFolder;
    public Summoner U_enemySummoner;
    public Warrior U_enemyWarrior;
    public Shieldman U_enemyShieldman;
    public Caster U_enemyCaster;
    public Archer U_enemyArcher;
    public GameObject enemyFolder;

    List<GameObject> playerSpawnPoints;
    List<GameObject> enemySpawnPoints;

    float squareSize;

    public void GenerateArmy(int width, int height, float size)
    {
        squareSize = size;
        float mapWidth = (width -1) * squareSize;
        float mapHeight = (height -1) * squareSize;
        float spawnWidth = mapWidth / 10;
        float spawnHeight = mapHeight / 10 ;

        int unitToSpawn = (int) ((width - 1)/10 * (height - 1)/10) / 2;

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
                BaseUnitController unit = Instantiate(toSpawn, playerSpawnPoints[position].transform.position, Quaternion.identity, playerFolder.transform);
                playerSpawnPoints[position].GetComponent<SquareTile>().currentUnit = unit;
                unit.bIsPlayer = true;
                playerSpawnPoints.RemoveAt(position);
            }
        }
        else
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                int position = Random.Range(0, enemySpawnPoints.Count - 1);
                BaseUnitController unit = Instantiate(toSpawn, enemySpawnPoints[position].transform.position, Quaternion.identity, enemyFolder.transform);
                enemySpawnPoints[position].GetComponent<SquareTile>().currentUnit = unit;
                unit.bIsPlayer = false;
                enemySpawnPoints.RemoveAt(position);
            }
        }        
    }
}
