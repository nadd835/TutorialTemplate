using UnityEngine;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using TMPro; 

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VoicePreGenerator : MonoBehaviour
{
    [Header("Text Objects to Generate Voice From")]
    public bool useTMP = false;                                   
    public List<Text> inputTextObjects;                           
    public List<TextMeshProUGUI> inputTMPTextObjects;           

    [Header("Output Folder (relative to TutorialTemplate/Resources)")]
    public string outputFolder = "VoiceClips";

    [ContextMenu("Generate Voice Clips From Text Objects")]
    public void GenerateVoiceClipsFromTextObjects()
    {
#if UNITY_EDITOR
        if (!useTMP && (inputTextObjects == null || inputTextObjects.Count == 0))
        {
            Debug.LogWarning("No UI Text objects assigned.");
            return;
        }
        if (useTMP && (inputTMPTextObjects == null || inputTMPTextObjects.Count == 0))
        {
            Debug.LogWarning("No TMP Text objects assigned.");
            return;
        }

        string fullPath = Path.Combine(Application.dataPath, "TutorialTemplate/Resources", outputFolder);

        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);

        HashSet<string> seenTexts = new HashSet<string>();
        int count = 0;

        if (useTMP)
        {
            // Iterate TMP list
            foreach (TextMeshProUGUI tmp in inputTMPTextObjects)
            {
                if (tmp != null && !string.IsNullOrWhiteSpace(tmp.text))
                {
                    GenerateClipFromText(tmp.text, fullPath, seenTexts, ref count);
                }
            }
        }
        else
        {
            // Iterate UI Text list
            foreach (Text text in inputTextObjects)
            {
                if (text != null && !string.IsNullOrWhiteSpace(text.text))
                {
                    GenerateClipFromText(text.text, fullPath, seenTexts, ref count);
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log($"Total new voice clips generated: {count}");
#else
        Debug.LogError("This method is only available in the Unity Editor.");
#endif
    }

    private void GenerateClipFromText(string text, string fullPath, HashSet<string> seenTexts, ref int count)
    {
        string cleanText = text.Trim();
        if (!seenTexts.Contains(cleanText))
        {
            seenTexts.Add(cleanText);

            string clipName = GetClipNameFromText(cleanText);
            string filePath = Path.Combine(fullPath, clipName + ".wav");

            if (!File.Exists(filePath))
            {
                Voice voice = Speaker.VoiceForCulture("en-US");
                if (voice != null)
                {
                    Speaker.Generate(cleanText, filePath, voice, 1f);
                    Debug.Log($"Generated: {clipName}.wav");
                    count++;
                }
                else
                {
                    Debug.LogError("No voice found for 'en-US' culture.");
                }
            }
            else
            {
                Debug.Log($"Skipped (already exists): {clipName}.wav");
            }
        }
    }

    private string GetClipNameFromText(string text)
    {
        return text.ToLowerInvariant()
                   .Trim()
                   .Replace(" ", "_")
                   .Replace(".", "")
                   .Replace(",", "")
                   .Replace("!", "")
                   .Replace("?", "")
                   .Replace(":", "")
                   .Replace(";", "")
                   .Replace("\"", "")
                   .Replace("'", "")
                   .Replace("(", "")
                   .Replace(")", "");
    }
}
