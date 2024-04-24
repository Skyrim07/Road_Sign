using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [Tooltip("Name of the music file you want to play")]
    [SerializeField] string track;
    [Tooltip("Whether previous music should be stopped")]
    [SerializeField] bool reset;
    void Start()
    {
        SKAudioManager.instance.StopMusic();
        SKAudioManager.instance.PlayMusic(track, reset, 0, 1);
    }
    private void Update()
    {
        if (RuntimeData.isPaused)
        {
            SKAudioManager.instance.ChangeMusicVolume(0.2f);
        }
        else
        {
            //SKAudioManager.instance.StopIdentifiableSound("pause loop");
            SKAudioManager.instance.ChangeMusicVolume(1f);
        }
    }
}
