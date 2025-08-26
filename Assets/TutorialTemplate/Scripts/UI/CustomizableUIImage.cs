using UnityEngine;
using UnityEngine.UI;

public class CustomizableUIImage : UICustomizableBase
{
    public Image target;

    public enum ImageType
    {
        PanelBackground,
        ButtonNormal,
        ButtonBack,
        ButtonModuleSelection,
        ButtonProgressBackground,
        WellDoneBackground    // ✅ New type
    }

    public ImageType imageType = ImageType.PanelBackground;

    public override void ApplyCustomization(UICustomizationData data)
    {
        if (target == null || data == null) return;

        switch (imageType)
        {
            case ImageType.PanelBackground:
                if (data.panelSprite != null) target.sprite = data.panelSprite;
                break;

            case ImageType.ButtonNormal:
                if (data.buttonSprite != null) target.sprite = data.buttonSprite;
                break;

            case ImageType.ButtonBack:
                if (data.backButtonSprite != null) target.sprite = data.backButtonSprite;
                break;

            case ImageType.ButtonModuleSelection:
                if (data.moduleSelectionSprite != null) target.sprite = data.moduleSelectionSprite;
                break;

            case ImageType.ButtonProgressBackground:
                if (data.progressBackgroundSprite != null) target.sprite = data.progressBackgroundSprite;
                break;

            case ImageType.WellDoneBackground:   // ✅ Apply new sprite
                if (data.wellDoneSprite != null) target.sprite = data.wellDoneSprite;
                break;
        }

        MarkDirty(target);
    }
}
