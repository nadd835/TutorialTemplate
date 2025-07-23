using UnityEngine;

public class TeleportController : MonoBehaviour
{
    [SerializeField] private GameObject[] teleportDestinations;

    private int currentTeleportIndex = 0;

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
            teleportDestinations[currentTeleportIndex].SetActive(true);
        }
        else
        {
            Debug.Log("No more teleport destinations to activate.");
        }
    }
}
