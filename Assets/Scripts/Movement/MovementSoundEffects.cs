using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSoundEffects : MonoBehaviour
{
    float playSpeed = 0.9f;
    List<AudioClip> clipList_step;
    List<AudioClip> clipList_jump;
    List<AudioClip> clipList_land;
    AudioClip previousClip;
    AudioSource playerAudio;

    CheckGroundTexture groundTexture;

    private bool isPlaying;

    public MovementSounds rockSounds;
    public MovementSounds grassSounds;
    public MovementSounds leafSounds;
    public MovementSounds soilSounds;
    public MovementSounds gravelSounds;

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
            //Debug.Log(values[0]+" "+ values[1] + " " + values[2] + " " + values[3] + " " + values[4] + " " + values[5] + " " + values[6] + " " + values[7]);
            SetSoundList(values);
            StartCoroutine(PlaySound(clipList_step));
        }
    }

    public void PlayJumpSound()
    {
        StopAllCoroutines();
        isPlaying = false;
        float[] values = groundTexture.GetValues();
        SetSoundList(values);
        StartCoroutine(PlaySound(clipList_jump));
        
    }

    public void PlayLandSound()
    {
        if (!isPlaying)
        {
            float[] values = groundTexture.GetValues();
            SetSoundList(values);
            StartCoroutine(PlaySound(clipList_land));
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
        yield return new WaitForSeconds(playerAudio.clip.length * playSpeed);

        isPlaying = false;
    }

    AudioClip GetClip(List<AudioClip> clipArray)
    {
        int attempts = 3;
        AudioClip selectedClip = clipArray[Random.Range(0, clipArray.Count - 1)];
        while (selectedClip == previousClip && attempts > 0)
        {
            selectedClip =
            clipArray[Random.Range(0, clipArray.Count - 1)];

            attempts--;
        }

        previousClip = selectedClip;
        return selectedClip;
    }

    public void setPlaySpeed(float s)
    {
        playSpeed = s;
    }

    void SetSoundList(float[] textureVals)
    {
        if (textureVals[0] > 0.5f)
        {
            clipList_step = gravelSounds.stepSounds;
            clipList_jump = gravelSounds.jumpSounds;
            clipList_land = gravelSounds.landSounds;
            return;
        }

        if (textureVals[1] > 0.5f)
        {
            clipList_step = soilSounds.stepSounds;
            clipList_jump = soilSounds.jumpSounds;
            clipList_land = soilSounds.landSounds;
        }

        if (textureVals[7] > 0.5f)
        {
            clipList_step = grassSounds.stepSounds;
            clipList_jump = grassSounds.jumpSounds;
            clipList_land = grassSounds.landSounds;
            return;
        }

        //indexes 2-6 are rock textures, so it is treated as the default sounds to play
        //if none of the above if statements are true
        clipList_step = rockSounds.stepSounds;
        clipList_jump = rockSounds.jumpSounds;
        clipList_land = rockSounds.landSounds;
    }
}
