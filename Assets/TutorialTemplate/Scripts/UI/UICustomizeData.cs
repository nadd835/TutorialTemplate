using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "UICustomizationData", menuName = "UI/UICustomizationData")]
public class UICustomizationData : ScriptableObject
{
    [Header("Text Colors")]
    public Color titleTextColor = Color.black;
    public Color generalTextColor = Color.black;
    public Color buttonTextColor = Color.black;

    [Header("Fonts (TMP & Legacy)")]
    public TMP_FontAsset tmpTitleFont;
    public Font legacyTitleFont;

    public TMP_FontAsset tmpGeneralFont;
    public Font legacyGeneralFont;

    public TMP_FontAsset tmpButtonFont;
    public Font legacyButtonFont;

    [Header("Sprites")]
    public Sprite panelSprite;
    public Sprite buttonSprite;
    public Sprite backButtonSprite;
    public Sprite progressBackgroundSprite;
    public Sprite moduleSelectionSprite;
}
