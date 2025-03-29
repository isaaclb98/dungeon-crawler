using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Singleton instance
    public Transform player;             // Reference to the player

    public float lastDeathLevel;         // To store level at death
    public float lastDeathGold;          // To store gold at death

    private bool restarting = false;     // Flag indicating that a restart is in progress

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate GameManager instances
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reassign the player if needed
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }

        // Only update InventoryManager on restart
        if (restarting)
        {
            GameObject newWeaponHolder = GameObject.Find("WeaponHolder");
            if (newWeaponHolder != null)
            {
                InventoryManager.Instance.UpdateWeaponHolder(newWeaponHolder.transform);
            }
            else
            {
                Debug.LogWarning("No object named 'WeaponHolder' found in the scene. Please ensure it exists.");
            }

            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.ResetInventory();
            }
            else
            {
                Debug.LogWarning("InventoryManager instance is null after scene load.");
            }

            restarting = false;
        }
    }

    public void LoadDeathScreen()
    {
        SceneManager.LoadScene("DeathScreen");
    }
    
    public void LoadWinScreen()
    {
        SceneManager.LoadScene("WinScreen"); 
        
    }

    public void RespawnGame()
    {
        restarting = true; 
        SceneManager.LoadScene("level1");  
    }
}
