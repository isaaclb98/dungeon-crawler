using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    public GameObject monsterPrefab; // The monster prefab to spawn
    public int maxMonsters = 5; // Maximum number of monsters per spawn point
    public float spawnDelay = 10f; // Delay between spawns
    public bool spawnOnStart = true; // Whether to start spawning automatically

    private int spawnedCount = 0; // How many monsters have been spawned

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnMonster(); // Spawn the first monster immediately
            if (maxMonsters > 1)
            {
                InvokeRepeating(nameof(SpawnMonster), spawnDelay, spawnDelay);
            }
        }
    }

    public void SpawnMonster()
    {
        if (spawnedCount < maxMonsters) // Check if we can still spawn
        {
            Instantiate(monsterPrefab, transform.position, transform.rotation);
            spawnedCount++;
        }
        else
        {
            CancelInvoke(nameof(SpawnMonster)); // Stop spawning once max is reached
        }
    }
}