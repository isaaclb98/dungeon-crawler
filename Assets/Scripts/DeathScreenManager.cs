using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenUI : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI goldText;
    public Button respawnButton;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Retrieve and display the death data stored in GameManager
        levelText.text = "" + GameManager.Instance.lastDeathLevel;
        goldText.text = "" + GameManager.Instance.lastDeathGold;

        // Add listener to the Respawn button
        respawnButton.onClick.AddListener(OnRespawn);
    }

    void OnRespawn()
    {
        Debug.Log("CLicked!");
        // Trigger the respawn process
        GameManager.Instance.RespawnGame();
    }
}