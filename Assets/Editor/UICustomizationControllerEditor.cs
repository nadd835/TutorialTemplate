using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UICustomizationController))]
public class UICustomizationControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UICustomizationController controller = (UICustomizationController)target;

        if (GUILayout.Button("Apply Customization"))
        {
            if (controller.customizationData != null)
            {
                controller.ApplyCustomizationToScene();
                EditorUtility.SetDirty(controller.gameObject);
                Debug.Log("Customization applied manually in Editor.");
            }
        }
    }
}

