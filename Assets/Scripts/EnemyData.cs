using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int enemyId;
    public int enemyHealth;
    public int enemyAttack;
    public int xpReward;
    public int goldReward;
}

