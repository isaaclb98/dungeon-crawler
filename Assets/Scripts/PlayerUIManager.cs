using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUIManager : MonoBehaviour
{
    public GameObject popupTextPrefab;
    public RectTransform popupTextContainer;

    public GameObject statsPanel;

    public TextMeshProUGUI level;
    public TextMeshProUGUI xp;
    public TextMeshProUGUI xptolevelup;
    public TextMeshProUGUI health;
    public TextMeshProUGUI maxhealth;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI defense;
    public TextMeshProUGUI gold;
    public void ToggleStatsPanel()
    {
        bool isActive = statsPanel.activeSelf;
        statsPanel.SetActive(!isActive);

        if (!isActive)
        {
            UpdateStatsText();
        }
    }

    private void UpdateStatsText()
    {
        var stats = PlayerStats.Instance;

        level.text = stats.currentLevel.ToString();
        xp.text = stats.currentXp.ToString();
        xptolevelup.text = stats.xpToLevelUp.ToString();
        health.text = stats.currentHealth.ToString();
        maxhealth.text = stats.currentMaxHealth.ToString();
        attack.text = stats.currentAttack.ToString();
        defense.text = stats.currentDefense.ToString();
        gold.text = stats.currentGold.ToString();
    }

    public void ShowPopupText(string message, Color color)
    {
        GameObject instance = Instantiate(popupTextPrefab, popupTextContainer);
        var popupText = instance.GetComponent<TextPopupUI>();
        popupText.SetText(message, color);
    }
}

