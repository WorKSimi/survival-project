using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class MobSpawning : NetworkBehaviour
{
    [SerializeField] private GameObject thisPlayer;
    [SerializeField] private RuleTile validTile; //Tile that mobs can spawn on
    public bool doMobSpawn = false; //Flag to see if mob spawn should happen

    private Vector3 playerPos;

    //private GameObject wallmapObject; //Object holding wall tilemap
    //private Tilemap wallTilemap; //The wall tilemap

    //private GameObject groundmapObject; //Object holding ground tilemap
    //private Tilemap groundTilemap; //The ground tilemap    

    //private bool canSpawnMob; //Bool to store if a mob can be spawned

    //public int currentSpawns;
    //public int maxSpawns = 5;
    public GameObject[] mobPool;

    //Search for valid spots to spawn based written code
    //Spawn mobs until it hits the waveLimit (Example, Wave Limit of 5, spawn mobs until it hits 5)
    //Once proper amount per wave is spawned, stop spawning and set timer

    //After timer is done, go again.

    public float waveCooldownTime = 120f; //Spawn a wave every 120 SECONDS / 2 MINUTES
    public int waveMobCount = 0; //Amount of mobs that are spawned already in wave
    public int waveMobMax = 5; //Amount of mobs to spawn in a wave
    public bool waveCanSpawn = false; //Flag to keep track if wave is ready or not. False by default.

    // Awake is called when object instantiated
    void Awake()
    {
        StartCoroutine(WaveCooldownTimer());
        //We don't want mobs to spawn at the start, so start with the cooldown timer
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = thisPlayer.transform.position;
        SpawnWave();
    }

    private void SpawnWave() //This function spawns the mobs in a wave
    {
        if (waveCanSpawn == true) //If the wave is allowed to spawn
        {
            while (waveMobCount < waveMobMax) //While the wave count is less than the max
            {
                if (IsHost)
                {
                    //This gets spawn location
                    float dist = Random.Range(10f, 30f); //Min dist from player, max dist from player
                    float angle = Random.Range(0, 360f);
                    Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * dist;
                    spawnPos = new Vector3(spawnPos.x + thisPlayer.transform.position.x, spawnPos.y + thisPlayer.transform.position.y, 0);

                    //Spawn mob at location
                    int element = Random.Range(0, mobPool.Length); //This gets mob from pool
                    var snail = Instantiate(mobPool[element], spawnPos, Quaternion.identity); //Instantiate chosen mob
                    snail.GetComponent<NetworkObject>().Spawn(); //Spawn on network
                    waveMobCount++; //Increment mob count
                }
                else if (IsClient)
                {
                    //Get Location
                    float dist = Random.Range(10f, 30f); //Min dist from player, max dist from player
                    float angle = Random.Range(0, 360f);
                    Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * dist;
                    spawnPos = new Vector3(spawnPos.x + thisPlayer.transform.position.x, spawnPos.y + thisPlayer.transform.position.y, 0);

                    //Get mob and pass into the RPC
                    int element = Random.Range(0, mobPool.Length); //This gets mob from pool
                    waveMobCount++; //Increment mob count
                    SpawnMobServerRpc(spawnPos, element); //Spawn on network
                }
            }
            //When wave mob count is not at max, start timer!
            StartCoroutine(WaveCooldownTimer());
        }
    }

    private IEnumerator WaveCooldownTimer()
    {
        waveCanSpawn = false;
        waveMobCount = 0;
        yield return new WaitForSeconds(waveCooldownTime); //Wait cooldown time
        waveCanSpawn = true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnMobServerRpc(Vector3 spawnPosition, int element)
    {
        var snail = Instantiate(mobPool[element], spawnPosition, Quaternion.identity);
        snail.GetComponent<NetworkObject>().Spawn();
    }

    //private void CheckIfCanSpawn(Vector3 spawnPos) //This function takes in a spawn position, and checks conditions to see if a spawn is possible
    //{
    //    canSpawnMob = true; //Sets it to true. If all conditions dont pass, it will STAY true. Otherwise, it wont spawn

    //    Vector3Int spawnPosInt = new Vector3Int(Mathf.FloorToInt(spawnPos.x), Mathf.FloorToInt(spawnPos.y), 0); //Get int of location for spawn

    //    if (wallTilemap.GetTile(spawnPosInt)) //If the spot has a wall tile there
    //    {
    //        Debug.Log("Cannot spawn mob, wall tile is occupied");
    //        canSpawnMob = false; //CANNOT spawn mob
    //    }

    //    if (currentSpawns == maxSpawns) //Current spawns equals Max Spawns
    //    {
    //        canSpawnMob = false; //CANNOT spawn mob
    //    }
    //}

    //void SpawnMob()
    //{
    //    if (Random.value >= 0.0017) // 1/600 chance, 1 - 0.9983 = 0.0017 [1/600 in decimal]
    //    {
    //        if (IsHost)
    //        {
    //            float dist = Random.Range(10f, 30f); //Min dist from player, max dist from player
    //            float angle = Random.Range(0, 360f);
    //            Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * dist;
    //            spawnPos = new Vector3(spawnPos.x + thisPlayer.transform.position.x, spawnPos.y + thisPlayer.transform.position.y, 0);
    //            CheckIfCanSpawn(spawnPos);
    //            if (canSpawnMob == false) return; //If can spawn mob is false (it failed the parameters) then go back

    //            //int number = Random.Range(0, 1);
    //            int element = Random.Range(0, mobPool.Length);

    //            var snail = Instantiate(mobPool[element], spawnPos, Quaternion.identity);
    //            snail.GetComponent<NetworkObject>().Spawn();
    //            currentSpawns++;
    //            //var snailScript = snail.GetComponent<SnailEnemy>();
    //            var snailHealthScript = snail.GetComponent<EnemyHealth>();
    //            //snailScript.playerWhoSpawned = thisPlayer;
    //            //snailScript.startingPosition = spawnPos;
    //            //snailScript.mobSpawning = this;
    //            //snailHealthScript.mobSpawning = this;
    //        }
    //        else if (IsClient)
    //        {
    //            float dist = Random.Range(10f, 30f); //Min dist from player, max dist from player
    //            float angle = Random.Range(0, 360f);
    //            Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * dist;
    //            spawnPos = new Vector3(spawnPos.x + thisPlayer.transform.position.x, spawnPos.y + thisPlayer.transform.position.y, 0);
    //            CheckIfCanSpawn(spawnPos);
    //            if (canSpawnMob == false) return; //If can spawn mob is false (it failed the parameters) then go back
    //            currentSpawns++;
    //            //SpawnMobServerRpc(spawnPos);
    //        }
    //    }       
    //}

}
