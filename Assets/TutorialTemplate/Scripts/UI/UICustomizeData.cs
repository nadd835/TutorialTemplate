using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "UICustomizationData", menuName = "UI/UICustomizationData")]
public class UICustomizationData : ScriptableObject
{
    [Header("Colors")]
    public Color titleTextColor = Color.black;
    public Color generalTextColor = Color.black;
    public Color buttonTextColor = Color.black;

    [Header("Fonts")]
    public TMP_FontAsset tmpTitleFont;
    public Font legacyTitleFont;
    public TMP_FontAsset tmpGeneralFont;
    public Font legacyGeneralFont;
    public TMP_FontAsset tmpButtonFont;
    public Font legacyButtonFont;

    [Header("Font Sizes")]
    public float titleFontSize = 36f;
    public float generalFontSize = 24f;
    public float buttonFontSize = 28f;

    [Header("Spritess")]

    public Sprite panelSprite;
    public Sprite buttonSprite;
    public Sprite backButtonSprite;
    public Sprite progressBackgroundSprite;
    public Sprite moduleSelectionSprite;
    public Sprite wellDoneSprite;
}
