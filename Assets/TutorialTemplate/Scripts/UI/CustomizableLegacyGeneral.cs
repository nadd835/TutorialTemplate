using UnityEngine;
using UnityEngine.UI;

public class CustomizableLegacyGeneral : UICustomizableBase
{
    public Text target;
    public bool useColor = true;
    public bool useFont = true;

    public override void ApplyCustomization(UICustomizationData data)
    {
        if (target == null || data == null) return;

        if (useColor) target.color = data.generalTextColor;
        if (useFont && data.legacyGeneralFont != null) target.font = data.legacyGeneralFont;

        MarkDirty(target);
    }
}
