using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int startingLevel = 1;
    public int startingHealth = 10;
    public int startingAttack = 1;
    public int startingDefense = 1;
    public int startingXp = 0;
    public int startingXpToLevelUp = 83;
    public double levelUpXpNeededMultiplier = 1.1;
    public int startingGold;
}
