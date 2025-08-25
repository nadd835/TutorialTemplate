using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VRWaypointManager : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;               
    public RectTransform canvasRect;          
    public GameObject waypointPrefab;         
    public bool showDistance = true;
    public float facingAngleThreshold = 15f;  
    
    [Header("Movement")]
    public float smoothingSpeed = 5f;        

    private List<WaypointData> waypoints = new List<WaypointData>();
    private Canvas canvas;
    private RectTransform canvasRectTransform;

    [System.Serializable]
    private class WaypointData
    {
        public Transform target;
        public RectTransform uiElement;
        public Text label;
        public Image arrow;
        public Vector2 targetPosition;        
    }

    void Start()
    {
        canvas = canvasRect.GetComponent<Canvas>();
        canvasRectTransform = canvasRect;
    }

    public void AddWaypoint(Transform target, string labelText)
    {
        GameObject wpUI = Instantiate(waypointPrefab, canvasRect);
        var label = wpUI.GetComponentInChildren<Text>();
        var arrow = wpUI.GetComponentInChildren<Image>();

        if (label != null) label.text = labelText;

        waypoints.Add(new WaypointData
        {
            target = target,
            uiElement = wpUI.GetComponent<RectTransform>(),
            label = label,
            arrow = arrow,
            targetPosition = Vector2.zero
        });
    }

    void LateUpdate()
    {
        foreach (var wp in waypoints)
        {
            if (wp.target == null) continue;

            bool targetActive = wp.target.gameObject.activeInHierarchy;
            if (!targetActive)
            {
                wp.uiElement.gameObject.SetActive(false);
                continue;
            }

            Vector3 dirToTarget = wp.target.position - playerCamera.transform.position;
            Vector3 flatDirToTarget = Vector3.ProjectOnPlane(dirToTarget, Vector3.up).normalized;
            Vector3 flatCameraForward = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized;

            float angleToTarget = Vector3.Angle(flatCameraForward, flatDirToTarget);
            bool facingTarget = angleToTarget <= facingAngleThreshold;

            wp.uiElement.gameObject.SetActive(!facingTarget);
            if (facingTarget) continue;

            Vector3 screenPoint = playerCamera.WorldToScreenPoint(wp.target.position);
            
            bool isBehind = screenPoint.z < 0;
            if (isBehind)
            {
                screenPoint.x = Screen.width - screenPoint.x;
                screenPoint.y = Screen.height - screenPoint.y;
            }

            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform, 
                screenPoint, 
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : playerCamera, 
                out canvasPos
            );

            Vector2 finalPosition = CalculateEdgePosition(canvasPos, screenPoint);

            wp.targetPosition = finalPosition;
            wp.uiElement.anchoredPosition = Vector2.Lerp(
                wp.uiElement.anchoredPosition, 
                wp.targetPosition, 
                Time.deltaTime * smoothingSpeed
            );

            if (wp.arrow != null)
            {
                Vector3 flatTargetDir = Vector3.ProjectOnPlane(dirToTarget, Vector3.up);
                float angle = Vector3.SignedAngle(flatCameraForward, flatTargetDir, Vector3.up);
                wp.arrow.rectTransform.localRotation = Quaternion.Euler(0, 0, -angle);
            }

            if (showDistance && wp.label != null)
            {
                float dist = Vector3.Distance(playerCamera.transform.position, wp.target.position);
                wp.label.text = $"{dist:F1}m";
            }
        }
    }

    private Vector2 CalculateEdgePosition(Vector2 canvasPos, Vector3 screenPoint)
    {
        Rect canvasRect = canvasRectTransform.rect;
        float halfWidth = canvasRect.width / 2f;
        float halfHeight = canvasRect.height / 2f;

        bool withinBounds = Mathf.Abs(canvasPos.x) <= halfWidth &&
                            Mathf.Abs(canvasPos.y) <= halfHeight;

        if (withinBounds && screenPoint.z > 0)
        {
            return canvasPos;
        }

        Vector2 directionFromCenter = canvasPos.normalized;
        if (directionFromCenter.magnitude < 0.01f)
        {
            directionFromCenter = Vector2.right;
        }

        float intersectX = 0;
        float intersectY = 0;
        
        float timeToRightEdge = directionFromCenter.x > 0 ? halfWidth / directionFromCenter.x : float.MaxValue;
        float timeToLeftEdge = directionFromCenter.x < 0 ? -halfWidth / directionFromCenter.x : float.MaxValue;
        float timeToTopEdge = directionFromCenter.y > 0 ? halfHeight / directionFromCenter.y : float.MaxValue;
        float timeToBottomEdge = directionFromCenter.y < 0 ? -halfHeight / directionFromCenter.y : float.MaxValue;

        float minTime = Mathf.Min(timeToRightEdge, timeToLeftEdge, timeToTopEdge, timeToBottomEdge);

        if (minTime == timeToRightEdge)
        {
            intersectX = halfWidth;
            intersectY = Mathf.Clamp(directionFromCenter.y * minTime, -halfHeight, halfHeight);
        }
        else if (minTime == timeToLeftEdge)
        {
            intersectX = -halfWidth;
            intersectY = Mathf.Clamp(directionFromCenter.y * minTime, -halfHeight, halfHeight);
        }
        else if (minTime == timeToTopEdge)
        {
            intersectY = halfHeight;
            intersectX = Mathf.Clamp(directionFromCenter.x * minTime, -halfWidth, halfWidth);
        }
        else if (minTime == timeToBottomEdge)
        {
            intersectY = -halfHeight;
            intersectX = Mathf.Clamp(directionFromCenter.x * minTime, -halfWidth, halfWidth);
        }

        return new Vector2(intersectX, intersectY);
    }

    public void RemoveWaypoint(Transform target)
    {
        for (int i = waypoints.Count - 1; i >= 0; i--)
        {
            if (waypoints[i].target == target)
            {
                if (waypoints[i].uiElement != null)
                {
                    Destroy(waypoints[i].uiElement.gameObject);
                }
                waypoints.RemoveAt(i);
                break;
            }
        }
    }

    public void ClearAllWaypoints()
    {
        foreach (var wp in waypoints)
        {
            if (wp.uiElement != null)
            {
                Destroy(wp.uiElement.gameObject);
            }
        }
        waypoints.Clear();
    }

    public Vector2 GetCanvasBounds()
    {
        Rect rect = canvasRectTransform.rect;
        return new Vector2(rect.width, rect.height);
    }
}

