using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGizmos : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3f); //Attacking Distance

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 8f); //Start chasing distance Distance

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 10f); //Stop chasing Distance
    }
}
