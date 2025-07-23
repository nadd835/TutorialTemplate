using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    [Header("Outline Settings")]
    [SerializeField] private Color selectionColor = Color.green;
    [SerializeField] private float outlineWidth = 7.0f;

    private Outline currentOutline;

    public void OnGrab(Transform grabbedObject)
    {
        // Early exit if the grabbed object is null or not selectable
        if (grabbedObject == null || !grabbedObject.CompareTag("Selectable"))
        {
            Debug.LogWarning("No grabbed object");
            return;

        }

        // Disable current outline if it exists
        DisableCurrentOutline();
        
        // Get or add outline component
        currentOutline = grabbedObject.GetComponent<Outline>() ?? grabbedObject.gameObject.AddComponent<Outline>();
        
        // Configure and enable outline
        currentOutline.OutlineColor = selectionColor;
        currentOutline.OutlineWidth = outlineWidth;
        currentOutline.enabled = true;
    }

    public void OnRelease(Transform releasedObject)
    {
        // Only disable if the current outline belongs to the released object
        if (currentOutline != null && releasedObject != null && 
            currentOutline.transform == releasedObject)
        {
            DisableCurrentOutline();
        }
    }

    private void DisableCurrentOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }

}