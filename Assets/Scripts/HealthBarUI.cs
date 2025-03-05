using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Transform creatureTransform;
    public RectTransform healthBarUI;
    public Camera mainCamera;

    void Update()
    {
        if(creatureTransform != null && healthBarUI != null)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(creatureTransform.position + Vector3.up * 2f);

            if(screenPosition.z > 0)
            {
                healthBarUI.position = screenPosition;
                healthBarUI.gameObject.SetActive(true);
            }
            else
            {
                healthBarUI.gameObject.SetActive(false);
            }
        }  
    }

}