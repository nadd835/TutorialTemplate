using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UICustomizableElement : MonoBehaviour
{
    [Header("Panel Backgrounds")]
    public List<Image> panelBackgrounds = new List<Image>();

    [Header("Title Text Settings")]
    public bool useTMPForTitle = true;
    public List<TMP_Text> tmpTitleLabels = new List<TMP_Text>();
    public List<Text> legacyTitleLabels = new List<Text>();

    [Header("General Text Settings")]
    public bool useTMPForGeneral = true;
    public List<TMP_Text> tmpGeneralLabels = new List<TMP_Text>();
    public List<Text> legacyGeneralLabels = new List<Text>();

    [Header("Button Text Settings")]
    public bool useTMPForButton = true;
    public List<TMP_Text> tmpButtonLabels = new List<TMP_Text>();
    public List<Text> legacyButtonLabels = new List<Text>();

    [Header("Button Images")]
    public List<Image> buttonImages = new List<Image>();
    public List<Image> backButtonImages = new List<Image>();
    public List<Image> progressIndicatorBackgrounds = new List<Image>();
    public List<Image> moduleSelectionBackgrounds = new List<Image>();

    public void ApplyCustomization(UICustomizationData data)
    {
        if (data == null) return;

        ApplySpriteList(panelBackgrounds, data.panelSprite);

        if (useTMPForTitle) ApplyTMPList(tmpTitleLabels, data.titleTextColor, data.tmpTitleFont);
        else ApplyLegacyList(legacyTitleLabels, data.titleTextColor, data.legacyTitleFont);

        if (useTMPForGeneral) ApplyTMPList(tmpGeneralLabels, data.generalTextColor, data.tmpGeneralFont);
        else ApplyLegacyList(legacyGeneralLabels, data.generalTextColor, data.legacyGeneralFont);

        if (useTMPForButton) ApplyTMPList(tmpButtonLabels, data.buttonTextColor, data.tmpButtonFont);
        else ApplyLegacyList(legacyButtonLabels, data.buttonTextColor, data.legacyButtonFont);

        ApplySpriteList(buttonImages, data.buttonSprite);
        ApplySpriteList(backButtonImages, data.backButtonSprite);
        ApplySpriteList(progressIndicatorBackgrounds, data.progressBackgroundSprite);
        ApplySpriteList(moduleSelectionBackgrounds, data.moduleSelectionSprite);

        MarkDirtyInEditor();
    }

    private void ApplySpriteList(List<Image> images, Sprite sprite)
    {
        if (sprite == null) return;
        foreach (var img in images) 
        {
            if (img != null) 
            {
                img.sprite = sprite;
                MarkDirtyInEditor(img);
            }
        }
    }

    private void ApplyTMPList(List<TMP_Text> texts, Color color, TMP_FontAsset font)
    {
        foreach (var t in texts)
        {
            if (t == null) continue;
            t.color = color;
            if (font != null) t.font = font;
            MarkDirtyInEditor(t);
        }
    }

    private void ApplyLegacyList(List<Text> texts, Color color, Font font)
    {
        foreach (var t in texts)
        {
            if (t == null) continue;
            t.color = color;
            if (font != null) t.font = font;
            MarkDirtyInEditor(t);
        }
    }

    private void MarkDirtyInEditor(Object obj = null)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (obj != null) EditorUtility.SetDirty(obj);
            else EditorUtility.SetDirty(this);
        }
#endif
    }
}