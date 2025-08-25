using UnityEngine;
using UnityEngine.UI;

public class CustomizableLegacyButton : UICustomizableBase
{
    public Text target;
    public bool useColor = true;
    public bool useFont = true;

    public override void ApplyCustomization(UICustomizationData data)
    {
        if (target == null || data == null) return;

        if (useColor) target.color = data.buttonTextColor;
        if (useFont && data.legacyButtonFont != null) target.font = data.legacyButtonFont;

        MarkDirty(target);
    }
}
