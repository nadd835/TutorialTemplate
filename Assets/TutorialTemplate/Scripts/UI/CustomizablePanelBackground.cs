using UnityEngine;
using UnityEngine.UI;

public class CustomizablePanelBackground : UICustomizableBase
{
    public Image target;

    public override void ApplyCustomization(UICustomizationData data)
    {
        if (target == null || data == null || data.panelSprite == null) return;

        target.sprite = data.panelSprite;
        MarkDirty(target);
    }
}
