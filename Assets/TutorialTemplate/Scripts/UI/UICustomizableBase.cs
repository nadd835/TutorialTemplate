using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class UICustomizableBase : MonoBehaviour
{
    public abstract void ApplyCustomization(UICustomizationData data);

    protected void MarkDirty(Object obj)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && obj != null)
            EditorUtility.SetDirty(obj);
#endif
    }
}
