using UnityEngine;
using TMPro;

public class CustomizableTMPButton : UICustomizableBase
{
    public TMP_Text target;
    public bool useColor = true;
    public bool useFont = true;

    public override void ApplyCustomization(UICustomizationData data)
    {
        if (target == null || data == null) return;

        if (useColor) target.color = data.buttonTextColor;
        if (useFont && data.tmpButtonFont != null) target.font = data.tmpButtonFont;

        MarkDirty(target);
    }
}
