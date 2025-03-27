using UnityEngine;

public class PressKeyDoor : MonoBehaviour
{
    public GameObject AnimeObject;
    public GameObject ThisTrigger;
    public bool Action;
    public bool isOpen = false;
    public PlayerUIManager uiManager;
    

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        uiManager = player.GetComponent<PlayerUIManager>();
    
        if (uiManager != null)
        {
            Debug.Log("UI Manager Found!");
        }
        else
        {
            Debug.Log("PlayerUIManager not found on Player.");
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!isOpen)
        {
            uiManager.ShowPopupText("Press E to open.", Color.white);
        }
        
        if (collision.transform.CompareTag("Player"))
        {
            Action = true;
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
                AnimeObject.GetComponent<Animator>().Play("DoorOpen");
                isOpen = true;
                ThisTrigger.SetActive(false);
                Action = false;
            }
        }

    }
}
