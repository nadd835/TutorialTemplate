using UnityEngine;

public class WaypointController : MonoBehaviour
{
    [Header("Waypoint Manager Reference")]
    public VRWaypointManager hud;
    
    [Header("Label")]
    public string label = "TELEPORT POINT";

    public void EnableWaypoint(Transform target)
    {
        if (target != null && hud != null)
        {
            hud.AddWaypoint(target, label);
        }
    }

    public void DisableWaypoint(Transform target)
    {
        if (target != null && hud != null)
        {
            hud.RemoveWaypoint(target);
        }
    }

    public void DisableAllWaypoints()
    {
        if (hud != null)
        {
            hud.ClearAllWaypoints();
        }
    }
}