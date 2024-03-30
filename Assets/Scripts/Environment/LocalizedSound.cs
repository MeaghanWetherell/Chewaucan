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

    private void Start()
    {
        isPlaying = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = looping;
    }

    IEnumerator PlayLocalizedSound(AudioClip clip)
    {
        isPlaying = true;

        if (clip != null)
        {
            Debug.Log("Playing " + clip.name + " with " + cooldown + " second cooldown");

            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length);
        }

        yield return new WaitForSeconds(cooldown);
        isPlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isPlaying && !looping)
            {
                int n = Random.Range(0, clipList.Count);
                AudioClip clip = clipList[n];
                StartCoroutine(PlayLocalizedSound(clip));
            }
            else if (looping)
            {
                int n = Random.Range(0, clipList.Count);
                AudioClip clip = clipList[n];
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (looping)
            {
                audioSource.Stop();
            }
        }
    }
}
