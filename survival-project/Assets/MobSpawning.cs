using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawning : MonoBehaviour
{
    [SerializeField] private GameObject thisPlayer;

    private Vector3 playerPos;
    public int currentSpawns;
    private int maxSpawns = 5;
    public GameObject[] mobPool;

    // Start is called before the first frame update
    void Awake()
    {
        currentSpawns = 0;
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = thisPlayer.transform.position;
        SpawnMob();
    }
    // if (Random.value >= 0.7) //30% chance, 1 - 0.7 = 0.3 (chance)

    void SpawnMob()
    {
        if (currentSpawns < maxSpawns)
        {
            if (Random.value >= 0.1) // 1/600 chance, 1 - 0.9983 = 0.0017 [1/600 in decimal]
            {
               
                float dist = Random.Range(10f, 30f); //Min dist from player, max dist from player
                float angle = Random.Range(0, 360f);
                Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * dist;
                spawnPos = new Vector3(spawnPos.x + thisPlayer.transform.position.x, spawnPos.y + thisPlayer.transform.position.y, 0);
                var snail = Instantiate(mobPool[0], spawnPos, Quaternion.identity);
                currentSpawns++;

                var snailScript = snail.GetComponent<SnailEnemy>();
                var snailHealthScript = snail.GetComponent<EnemyHealth>();
                snailScript.playerWhoSpawned = thisPlayer;
                snailScript.startingPosition = spawnPos;

                snailScript.mobSpawning = this;
                snailHealthScript.mobSpawning = this;
            }
        }
    }
}
