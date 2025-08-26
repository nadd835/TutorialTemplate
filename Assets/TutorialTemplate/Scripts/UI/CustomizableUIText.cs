using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomizableUIText : UICustomizableBase
{
    public enum TextType
    {
        Title,
        General,
        Button
    }

    [Header("Target Texts")]
    public TMP_Text tmpText;     
    public Text legacyText;      

    public TextType textType = TextType.General;
    public bool overrideFontSize = false;
    public float customFontSize = 24f;

    public override void ApplyCustomization(UICustomizationData data)
    {
        if (data == null) return;

        Color textColor = Color.white;
        TMP_FontAsset tmpFont = null;
        Font legacyFont = null;
        float fontSize = 0f;

        switch (textType)
        {
            case TextType.Title:
                textColor = data.titleTextColor;
                tmpFont = data.tmpTitleFont;
                legacyFont = data.legacyTitleFont;
                fontSize = data.titleFontSize;
                break;

            case TextType.General:
                textColor = data.generalTextColor;
                tmpFont = data.tmpGeneralFont;
                legacyFont = data.legacyGeneralFont;
                fontSize = data.generalFontSize;
                break;

            case TextType.Button:
                textColor = data.buttonTextColor;
                tmpFont = data.tmpButtonFont;
                legacyFont = data.legacyButtonFont;
                fontSize = data.buttonFontSize;
                break;
        }

        ApplyText(textColor, tmpFont, legacyFont, fontSize);
    }

    private void ApplyText(Color color, TMP_FontAsset tmpFont, Font legacyFont, float defaultFontSize)
    {
        float sizeToApply = overrideFontSize ? customFontSize : defaultFontSize;

        // TMP
        if (tmpText != null)
        {
            tmpText.color = color;
            if (tmpFont != null) tmpText.font = tmpFont;
            if (sizeToApply > 0f) tmpText.fontSize = sizeToApply;
            MarkDirty(tmpText);
        }

        // Legacy
        if (legacyText != null)
        {
            legacyText.color = color;
            if (legacyFont != null) legacyText.font = legacyFont;
            if (sizeToApply > 0f) legacyText.fontSize = Mathf.RoundToInt(sizeToApply);
            MarkDirty(legacyText);
        }
    }
}
