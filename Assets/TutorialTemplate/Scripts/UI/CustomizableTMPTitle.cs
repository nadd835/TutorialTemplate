using UnityEngine;
using TMPro;

public class CustomizableTMPTitle : UICustomizableBase
{
    public TMP_Text target;
    public bool useColor = true;
    public bool useFont = true;

    public override void ApplyCustomization(UICustomizationData data)
    {
        if (target == null || data == null) return;

        if (useColor) target.color = data.titleTextColor;
        if (useFont && data.tmpTitleFont != null) target.font = data.tmpTitleFont;

        MarkDirty(target);
    }
}