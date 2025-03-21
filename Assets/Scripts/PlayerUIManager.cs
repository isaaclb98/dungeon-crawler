using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject popupTextPrefab;
    public RectTransform popupTextContainer;

    public void ShowPopupText(string message, Color color)
    {
        GameObject instance = Instantiate(popupTextPrefab, popupTextContainer);
        var popupText = instance.GetComponent<TextPopupUI>();
        popupText.SetText(message, color);
    }
}

