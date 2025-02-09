using UnityEngine;

public static class RaycastUtility
{
    public static bool PerformRaycast(Camera playerCamera, out RaycastHit hit, float range)
    {
        if (playerCamera)
            return Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range);
        Debug.LogError("RaycastUtility: playerCamera is null!");
        hit = default;
        return false;
    }
}


