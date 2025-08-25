using UnityEngine;
using TMPro;

public class CustomizableTMPGeneral : UICustomizableBase
{
    public TMP_Text target;
    public bool useColor = true;
    public bool useFont = true;

    public override void ApplyCustomization(UICustomizationData data)
    {
        if (target == null || data == null) return;

        if (useColor) target.color = data.generalTextColor;
        if (useFont && data.tmpGeneralFont != null) target.font = data.tmpGeneralFont;

        MarkDirty(target);
    }
}
