using System.Collections.Generic;
using UnityEngine;
using Audio;

public class BGMManager : MonoBehaviour
{
    
    [Tooltip("Main theme background music clip")]
    public List<AudioClip> bgm;

    public void StartMusic()
    {
        SoundManager.soundManager.SetBGM(bgm);
    }
    
    public void StopMusic()
    {
        SoundManager.soundManager.StopBGM();
    }

    private void Start()
    {
        StartMusic();
    }


}
