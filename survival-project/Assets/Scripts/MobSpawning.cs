using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class MobSpawning : NetworkBehaviour
{
    [SerializeField] private GameObject thisPlayer;
    [SerializeField] private RuleTile validTile; //Tile that mobs can spawn on

    private Vector3 playerPos;

    private GameObject wallmapObject; //Object holding wall tilemap
    private Tilemap wallTilemap; //The wall tilemap

    private GameObject groundmapObject; //Object holding ground tilemap
    private Tilemap groundTilemap; //The ground tilemap    

    private bool canSpawnMob; //Bool to store if a mob can be spawned

    public int currentSpawns;
    public int maxSpawns = 5;
    public GameObject[] mobPool;

    // Awake is called when object instantiated
    void Awake()
    {
        currentSpawns = 0;

        wallmapObject = GameObject.FindWithTag("WallTilemap"); //Get Wall Tilemap Object
        wallTilemap = wallmapObject.GetComponent<Tilemap>(); //Get wall tilemap

        groundmapObject = GameObject.FindWithTag("GroundTilemap"); //Get Ground Tilemap Object
        groundTilemap = groundmapObject.GetComponent<Tilemap>(); //Get ground tilemap
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = thisPlayer.transform.position;
        SpawnMob();
    }

    private void CheckIfCanSpawn(Vector3 spawnPos) //This function takes in a spawn position, and checks conditions to see if a spawn is possible
    {
        canSpawnMob = true; //Sets it to true. If all conditions dont pass, it will STAY true. Otherwise, it wont spawn

        Vector3Int spawnPosInt = new Vector3Int(Mathf.FloorToInt(spawnPos.x), Mathf.FloorToInt(spawnPos.y), 0); //Get int of location for spawn
        //var groundTile = groundTilemap.GetTile(spawnPosInt); //Get ground tile

        if (wallTilemap.GetTile(spawnPosInt)) //If the spot has a wall tile there
        {
            Debug.Log("Cannot spawn mob, wall tile is occupied");
            canSpawnMob = false; //CANNOT spawn mob
        }

        if (currentSpawns == maxSpawns) //Current spawns equals Max Spawns
        {
            canSpawnMob = false; //CANNOT spawn mob
        }
    }

    void SpawnMob()
    {
        if (Random.value >= 0.0017) // 1/600 chance, 1 - 0.9983 = 0.0017 [1/600 in decimal]
        {
            if (IsHost)
            {
                float dist = Random.Range(10f, 30f); //Min dist from player, max dist from player
                float angle = Random.Range(0, 360f);
                Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * dist;
                spawnPos = new Vector3(spawnPos.x + thisPlayer.transform.position.x, spawnPos.y + thisPlayer.transform.position.y, 0);
                CheckIfCanSpawn(spawnPos);
                if (canSpawnMob == false) return; //If can spawn mob is false (it failed the parameters) then go back

                int number = Random.Range(0, 1);
                int element = Random.Range(0, mobPool.Length);

                var snail = Instantiate(mobPool[element], spawnPos, Quaternion.identity);
                snail.GetComponent<NetworkObject>().Spawn();
                currentSpawns++;
                var snailScript = snail.GetComponent<SnailEnemy>();
                var snailHealthScript = snail.GetComponent<EnemyHealth>();
                //snailScript.playerWhoSpawned = thisPlayer;
                //snailScript.startingPosition = spawnPos;
                //snailScript.mobSpawning = this;
                snailHealthScript.mobSpawning = this;
            }
            else if (IsClient)
            {
                float dist = Random.Range(10f, 30f); //Min dist from player, max dist from player
                float angle = Random.Range(0, 360f);
                Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * dist;
                spawnPos = new Vector3(spawnPos.x + thisPlayer.transform.position.x, spawnPos.y + thisPlayer.transform.position.y, 0);
                CheckIfCanSpawn(spawnPos);
                if (canSpawnMob == false) return; //If can spawn mob is false (it failed the parameters) then go back
                currentSpawns++;
                SpawnMobServerRpc(spawnPos);
            }
        }       
    }
    [ServerRpc(RequireOwnership = false)]
    private void SpawnMobServerRpc(Vector3 spawnPosition)
    {
        var snail = Instantiate(mobPool[0], spawnPosition, Quaternion.identity);
        snail.GetComponent<NetworkObject>().Spawn();
        var snailScript = snail.GetComponent<SnailEnemy>();
        var snailHealthScript = snail.GetComponent<EnemyHealth>();
        //snailScript.playerWhoSpawned = thisPlayer;
        //snailScript.startingPosition = spawnPosition;
        //snailScript.mobSpawning = this;
        snailHealthScript.mobSpawning = this;
    }
}
