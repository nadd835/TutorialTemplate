using UnityEngine;

public class UICustomizationController : MonoBehaviour
{
    public UICustomizationData customizationData;
    public bool applyOnAwake = true;

    private void Awake()
    {
        if (applyOnAwake && customizationData != null)
            ApplyCustomizationToScene();
    }

    public void ApplyCustomizationToScene()
    {
        var customizables = FindObjectsOfType<UICustomizableBase>(true);

        foreach (var c in customizables)
        {
            c.ApplyCustomization(customizationData);
        }

        Debug.Log($"[UI Customization] Applied customization to {customizables.Length} elements.");
    }
}
