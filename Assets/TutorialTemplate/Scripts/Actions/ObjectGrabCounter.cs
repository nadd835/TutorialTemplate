using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BNG {
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

    private bool grabTargetReached = false;
    private bool storeTargetReached = false;

    private void Start()
    {
        // Initialize flags
        grabTargetReached = false;
        storeTargetReached = false;
    }


    // Method to increase the grab count
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

    //Check if the object should be activated
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
    }
}