
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BNG {

[System.Serializable]
public class TrackedObject
{
    public GameObject gameObject;
    public Vector3 initialPosition;
    public Quaternion initialRotation;
    public Transform initialParent;
    
    public TrackedObject(GameObject obj)
    {
        gameObject = obj;
        if (obj != null)
        {
            initialPosition = obj.transform.position;
            initialRotation = obj.transform.rotation;
            initialParent = obj.transform.parent;
        }
    }
}

public class ObjectGrabCounter : MonoBehaviour
{
    [Header("Grab Settings")]
    [SerializeField] private int grabCount = 0;
    [SerializeField] private int targetGrabCount = 2;
    public UnityEvent onTargetGrabReached;

    [Header("Store Settings")]
    [SerializeField] private int storeCount = 0;
    [SerializeField] private int targetStoreCount = 2;
    public UnityEvent onTargetStoreReached;

    [Header("Object Tracking")]
    [SerializeField] private List<GameObject> objectsToTrack = new List<GameObject>();
    private List<TrackedObject> trackedObjects = new List<TrackedObject>();

    private bool grabTargetReached = false;
    private bool storeTargetReached = false;

    private void Start()
    {
        grabTargetReached = false;
        storeTargetReached = false;
        
        InitializeTrackedObjects();
    }

    private void InitializeTrackedObjects()
    {
        trackedObjects.Clear();
        
        foreach (GameObject obj in objectsToTrack)
        {
            if (obj != null)
            {
                trackedObjects.Add(new TrackedObject(obj));
            }
        }
        
        Debug.Log($"Initialized {trackedObjects.Count} tracked objects for '{gameObject.name}'");
    }
    
    public void ResetObjectPositions()
    {
        int resetCount = 0;
        
        foreach (TrackedObject trackedObj in trackedObjects)
        {
            if (trackedObj.gameObject != null)
            {
                // Reset position and rotation
                trackedObj.gameObject.transform.position = trackedObj.initialPosition;
                trackedObj.gameObject.transform.rotation = trackedObj.initialRotation;
                
                // Reset parent if it was changed
                if (trackedObj.gameObject.transform.parent != trackedObj.initialParent)
                {
                    trackedObj.gameObject.transform.SetParent(trackedObj.initialParent);
                }
                
                // Reset rigidbody velocity if present
                Rigidbody rb = trackedObj.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                
                resetCount++;
            }
        }
        
        Debug.Log($"Reset {resetCount} objects to their initial positions");
    }


    public void IncreaseGrabCount()
    {
        grabCount++;
        CheckGrabActivation();
    }
    
    public void DecreaseGrabCount()
    {
        grabCount--;
        if (grabCount < 0) grabCount = 0;
    }

    private void CheckGrabActivation()
    {
        if (grabCount >= targetGrabCount && !grabTargetReached)
        {
            grabTargetReached = true;
            onTargetGrabReached?.Invoke();
        }
    }

    public void IncreaseStoreCount()
    {
        storeCount++;
        CheckStoreActivation();
    }

    public void DecreaseStoreCount()
    {
        storeCount--;
        if (storeCount < 0) storeCount = 0;
    }

    private void CheckStoreActivation()
    {
        if (storeCount >= targetStoreCount && !storeTargetReached)
        {
            storeTargetReached = true;
            onTargetStoreReached?.Invoke();
        }
    }
    

    public void ResetGrab()
    {
        grabCount = 0;
        grabTargetReached = false;
        
        Debug.Log($"ObjectGrabCounter '{gameObject.name}' grab settings reset");
    }

    public void ResetStore() 
    {
        storeCount = 0;
        storeTargetReached = false;
        
        Debug.Log($"ObjectGrabCounter '{gameObject.name}' store settings reset");
    }

    public int GetGrabCount() => grabCount;
    public int GetStoreCount() => storeCount;
    public bool IsGrabTargetReached() => grabTargetReached;
    public bool IsStoreTargetReached() => storeTargetReached;
    public float GetGrabProgress() => targetGrabCount > 0 ? (float)grabCount / targetGrabCount : 0f;
    public float GetStoreProgress() => targetStoreCount > 0 ? (float)storeCount / targetStoreCount : 0f;
    public List<GameObject> GetTrackedObjects() => new List<GameObject>(objectsToTrack);
    public int GetTrackedObjectCount() => trackedObjects.Count;
}
}