using System.Collections;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    public GameObject monsterPrefab; 
    public float spawnInterval = 5f; 
    public float detectionRadius = 10f; 
    public int maxHitsToDestroy = 4;

    private int hitCount = 0; 
    private GameObject player;

    [HideInInspector] public PlayerUIManager uiManager;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        uiManager = player.GetComponent<PlayerUIManager>();
        StartCoroutine(SpawnEnemies());
    }

    // Spawn enemies at regular intervals
    IEnumerator SpawnEnemies()
    {
        while (hitCount < maxHitsToDestroy) 
        {
            // If player is within radius and spawner is still alive
            if (player != null && Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
            {
                // Instantiate the monster prefab and spawn it
                Instantiate(monsterPrefab, transform.position, Quaternion.identity);
                // Wait for the next spawn interval
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                // Wait before checking again (if player is not within range)
                yield return null;
            }
        }
    }

    public void TakeDamage()
    {
        hitCount++;

        if (hitCount >= maxHitsToDestroy)
        {
            DestroySpawner();
            return;
        }
        
        uiManager.ShowPopupText("Enemy Spawner: " + (maxHitsToDestroy - hitCount) + " hits left to be destroyed!", Color.magenta);
    }

    // Destroy the spawner
    void DestroySpawner()
    {
        uiManager.ShowPopupText("Enemy Spawner destroyed!", Color.magenta);

        Destroy(gameObject); 
    }

    private void OnDrawGizmosSelected()
    {
        // Show the spawn radius in the scene view for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
