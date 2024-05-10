using Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class LocalizedSound : MonoBehaviour
{
    [Tooltip("List of audio clips that may play when this is triggered")]
    public List<AudioClip> clipList = new();

    [Min(0)]
    public float cooldown = 0f;

    public bool looping = false;

    private AudioSource audioSource;
    private bool isPlaying;
    private bool paused;
    private bool exited;

    private void Start()
    {
        isPlaying = false;
        exited = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = looping;

        PauseCallback.pauseManager.SubscribeToPause(OnPause);
        PauseCallback.pauseManager.SubscribeToResume(OnResume);
    }

    private void OnDestroy()
    {
        PauseCallback.pauseManager.UnsubToPause(OnPause);
        PauseCallback.pauseManager.UnsubToResume(OnResume);
    }

    //plays a localized sound, waits until the player exits the collider, and then waits for the cooldown
    IEnumerator PlayLocalizedSound(AudioClip clip)
    {
        isPlaying = true;

        if (clip != null)
        {
            Debug.Log("Playing " + clip.name + " with " + cooldown + " second cooldown");

            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitUntil(() => (!audioSource.isPlaying && !paused));
        }

        yield return new WaitUntil(() => (exited));
        yield return new WaitForSeconds(cooldown);
        isPlaying = false;
    }

    //plays a random clip from the list when the player enters the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isPlaying)
            {
                int n = Random.Range(0, clipList.Count);
                AudioClip clip = clipList[n];
                exited = false;
                StartCoroutine(PlayLocalizedSound(clip));
            }
        }
    }

    // sets exited bool to true and stops looping audio when the player exits the collider
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            exited = true;
            if (looping)
            {
                audioSource.Stop();
            }
        }
    }

    // pauses audio clip and sets boolean so the coroutine does not continue
    private void OnPause()
    {
        audioSource.Pause();
        paused = true;
    }

    // unpauses and set boolean so coroutine can continue
    private void OnResume()
    {
        audioSource.UnPause();
        paused = false;
    }
}
