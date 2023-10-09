using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSoundEffects : MonoBehaviour
{
    public List<AudioClip> clipList_soil;
    AudioClip previousClip;
    AudioSource playerAudio;

    CheckGroundTexture groundTexture;

    private bool isPlaying;

    private void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        groundTexture = GetComponent<CheckGroundTexture>();
        isPlaying = false;
    }

    //Plays the walking sounds if the coroutine is not already running
    public void PlayWalkingSound()
    {
        if (!isPlaying)
        {
            float[] values = groundTexture.GetValues();
            Debug.Log(values[0]+" "+ values[1] + " " + values[2] + " " + values[3] + " " + values[4] + " " + values[5] + " " + values[6] + " " + values[7]);
            StartCoroutine(PlaySound(clipList_soil));
        }
    }

    /**
     * Plays an audio clip and waits for it to finish playing.
     */
    IEnumerator PlaySound(List<AudioClip> clipList)
    {
        isPlaying = true;

        AudioClip clip = GetClip(clipList);

        playerAudio.clip = clip;
        playerAudio.Play();
        yield return new WaitForSeconds(playerAudio.clip.length * 0.9f);

        isPlaying = false;
    }

    AudioClip GetClip(List<AudioClip> clipArray)
    {
        int attempts = 3;
        AudioClip selectedClip =
        clipArray[Random.Range(0, clipArray.Count - 1)];
        while (selectedClip == previousClip && attempts > 0)
        {
            selectedClip =
            clipArray[Random.Range(0, clipArray.Count - 1)];

            attempts--;
        }

        previousClip = selectedClip;
        return selectedClip;
    }


}
