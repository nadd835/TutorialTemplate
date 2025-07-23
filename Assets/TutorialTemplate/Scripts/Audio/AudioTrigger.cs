using System.Collections;
using CS.AudioToolkit;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public void Play(string audioId)
    {
        AudioController.Play(audioId);
    }

    public void Stop(string audioId) 
    {
        AudioController.Stop(audioId);
    }
}

