using UnityEngine;

public class PressKeyDoor : MonoBehaviour
{ 
    [HideInInspector] public GameObject AnimeObject;
    [HideInInspector] public GameObject ThisTrigger;
    [HideInInspector] public bool Action;
    [HideInInspector] public bool isOpen = false;
    [HideInInspector] public PlayerUIManager uiManager;
    [HideInInspector] public InventoryManager inventoryManager;
    public bool isLocked = false;
    
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        uiManager = player.GetComponent<PlayerUIManager>();
        inventoryManager = InventoryManager.Instance;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Action = true;
            
            if (!isOpen)
            {
                uiManager.ShowPopupText("Press E to open.", Color.white);
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        Action = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Action)
            {
                if (!isLocked)
                {
                    AnimeObject.GetComponent<Animator>().Play("DoorOpen");
                    isOpen = true;
                    ThisTrigger.SetActive(false);
                    Action = false;
                }
                else
                {
                    bool haveKey = false;
                    KeyItem keyItemFound = null;
                    foreach (ItemData item in inventoryManager.inventoryItems)
                    {
                        Debug.Log(item.itemName);
                        if (item.itemName == "Key")
                        {
                            Debug.Log("Key found!");
                            haveKey = true;
                            keyItemFound = item as KeyItem;
                            break;
                        }
                    }
                    
                    Debug.Log("keyItemFound is " + keyItemFound);
                    
                    if (haveKey && keyItemFound)
                    {
                        keyItemFound.RemoveItem();
                        
                        // Now unlock and open the door.
                        isLocked = false;
                        AnimeObject.GetComponent<Animator>().Play("DoorOpen");
                        isOpen = true;
                        ThisTrigger.SetActive(false);
                        Action = false;
                        uiManager.ShowPopupText("Used a key.", Color.white);
                    }
                    else
                    {
                        uiManager.ShowPopupText("Door is locked. You need a key.", Color.white);
                    }
                }
            }
        }
    }
}
