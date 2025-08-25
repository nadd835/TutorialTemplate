using UnityEngine;
using UnityEngine.Events;

public class TeleportController : MonoBehaviour
{
    [SerializeField] private GameObject[] teleportDestinations;
    [SerializeField] private UnityEvent<GameObject> onTeleportPointActivated;

    private int currentTeleportIndex = 0;

    public void ActivateFirstTeleportDestination()
    {
        if (teleportDestinations != null &&
            teleportDestinations.Length > 0 &&
            teleportDestinations[0] != null)
        {
            // Ensure all are disabled first
            foreach (var point in teleportDestinations)
            {
                if (point != null) point.SetActive(false);
            }

            // Activate the first one
            teleportDestinations[0].SetActive(true);
            onTeleportPointActivated?.Invoke(teleportDestinations[0]);

            currentTeleportIndex = 0;
        }
    }

    public void ActivateNextTeleportDestination()
    {
        if (teleportDestinations != null &&
            currentTeleportIndex < teleportDestinations.Length &&
            teleportDestinations[currentTeleportIndex] != null)
        {
            teleportDestinations[currentTeleportIndex].SetActive(false);
        }

        currentTeleportIndex++;

        if (teleportDestinations != null &&
            currentTeleportIndex < teleportDestinations.Length &&
            teleportDestinations[currentTeleportIndex] != null)
        {
            GameObject nextPoint = teleportDestinations[currentTeleportIndex];
            nextPoint.SetActive(true);

            onTeleportPointActivated?.Invoke(nextPoint);
        }
        else
        {
            Debug.Log("No more teleport destinations to activate.");
        }
    }
    
    public void ResetTeleport()
    {
        if (teleportDestinations != null)
        {
            foreach (var point in teleportDestinations)
            {
                if (point != null) point.SetActive(false);
            }
        }

        currentTeleportIndex = 0;
    }
}
