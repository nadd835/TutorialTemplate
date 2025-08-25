using UnityEngine;
using UnityEngine.UI;

public class CustomizableLegacyTitle : UICustomizableBase
{
    public Text target;
    public bool useColor = true;
    public bool useFont = true;

    public override void ApplyCustomization(UICustomizationData data)
    {
        if (target == null || data == null) return;

        if (useColor) target.color = data.titleTextColor;
        if (useFont && data.legacyTitleFont != null) target.font = data.legacyTitleFont;

        MarkDirty(target);
    }
}
