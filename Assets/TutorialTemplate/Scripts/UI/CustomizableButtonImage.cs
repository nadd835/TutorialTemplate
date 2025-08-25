using UnityEngine;
using UnityEngine.UI;

public class CustomizableButtonImage : UICustomizableBase
{
    public Image target;

    public enum ButtonType { Normal, Back, ModuleSelection, ProgressBackground }
    public ButtonType buttonType = ButtonType.Normal;

    public override void ApplyCustomization(UICustomizationData data)
    {
        if (target == null || data == null) return;

        switch (buttonType)
        {
            case ButtonType.Back:
                if (data.backButtonSprite != null) target.sprite = data.backButtonSprite;
                break;
            case ButtonType.ModuleSelection:
                if (data.moduleSelectionSprite != null) target.sprite = data.moduleSelectionSprite;
                break;
            case ButtonType.ProgressBackground:
                if (data.progressBackgroundSprite != null) target.sprite = data.progressBackgroundSprite;
                break;
            default:
                if (data.buttonSprite != null) target.sprite = data.buttonSprite;
                break;
        }

        MarkDirty(target);
    }
}
