using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSoundEffects : MonoBehaviour
{
    public List<AudioClip> clipList;
    AudioSource playerAudio;
    int currSoundPlaying;

    private bool isPlaying;

    private void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        currSoundPlaying = 0;
        isPlaying = false;
    }

    //Plays the walking sounds if the coroutine is not already running
    public void PlayWalkingSound()
    {
        if (!isPlaying)
        {
            StartCoroutine(nameof(PlaySound));
        }
    }

    /**
     * Plays an audio clip and waits for it to finish playing.
     */
    IEnumerator PlaySound()
    {
        isPlaying = true;

        if (currSoundPlaying >= clipList.Count)
        {
            currSoundPlaying = 0;
        }

        playerAudio.clip = clipList[currSoundPlaying];
        playerAudio.Play();
        yield return new WaitForSeconds(playerAudio.clip.length * 0.9f);
        currSoundPlaying++;

        isPlaying = false;
    }


}
