using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        if (restarting)
        {
            Debug.Log("▶ Scene loaded after restart. Reinitializing...");

            // ✅ Safely get the new player
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
                Debug.Log("✅ Player reassigned.");
            }
            else
            {
                Debug.LogError("❌ No player found in scene after restart.");
                return; // Stop here to avoid using a null player reference
            }

            // Reassign weapon holder
            GameObject newWeaponHolder = GameObject.Find("WeaponHolder");
            if (newWeaponHolder != null && InventoryManager.Instance != null)
            {
                InventoryManager.Instance.UpdateWeaponHolder(newWeaponHolder.transform);
            }

            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.ResetInventory();
            }

            // Safely access UI under new player
            Transform pausemenu = player.Find("Canvas/PauseMenu");
            if (pausemenu != null)
            {
                Transform inventoryMenu = player.Find("Canvas/InventoryMenu");
                Transform itemContent = inventoryMenu?.Find("Viewport/Content");

                if (itemContent != null)
                {
                    InventoryManager.Instance.ItemContent = itemContent;
                    Debug.Log("✅ ItemContent reassigned.");
                }
                else
                {
                    Debug.LogWarning("❌ Could not find Viewport/Content.");
                }

                // Re-hook InventoryButton
                Button inventoryButton = pausemenu.Find("inventoryButton")?.GetComponent<Button>();
                if (inventoryButton != null)
                {
                    inventoryButton.onClick.AddListener(InventoryManager.Instance.ListItems);
                    Debug.Log("✅ AddListener called for ListItems()");
                }
                else
                {
                    Debug.LogWarning("❌ InventoryButton not found.");
                }
            }
            else
            {
                Debug.LogWarning("❌ PauseMenu not found under new player's Canvas.");
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
