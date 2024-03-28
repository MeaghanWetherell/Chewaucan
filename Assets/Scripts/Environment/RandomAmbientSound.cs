using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomAmbientSound : MonoBehaviour
{
    [Tooltip("Audio clips to play in both maps")]
    public List<RandomAmbientSoundObject> generalAmbientSounds = new List<RandomAmbientSoundObject>();

    [Tooltip("Audio clips to play only in the modern map")]
    public List<RandomAmbientSoundObject> modernMapAmbientSounds = new List<RandomAmbientSoundObject>();

    // not using the pleistocene map much yet, but might need it in the future
    [Tooltip("Audio clips to play only in the pleistocene map")]
    public List<RandomAmbientSoundObject> pastMapAmbientSounds = new List<RandomAmbientSoundObject>();

    [Tooltip("Shortest time between sound effects (in seconds)")]
    [Min(0.1f)]
    public float minCooldown = 2f;

    [Tooltip("Longest time between sound effects (in seconds)")]
    [Min(0.1f)]
    public float maxCooldown = 10f;

    private AudioSource audioSource;

    private List<RandomAmbientSoundObject> currentSoundList = new List<RandomAmbientSoundObject>();
    private List<int> frequencyList = new List<int>();

    private bool playing;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playing = false;
        StartCoroutine(PlayAmbientSound(null));

        List<List<RandomAmbientSoundObject>> soundLists = new()
        {
            GetMapSpecificSounds(),
            generalAmbientSounds
        };

        currentSoundList = GetAllPlayableSounds(soundLists);
    }

    private void Update()
    {
        if (!playing)
        {
            ChooseRandomSound();
        }
    }

    private void ChooseRandomSound()
    {
        MakeFrequencyAccurateList(currentSoundList);
        int n = Random.Range(0, frequencyList.Count);
        int soundIndex = frequencyList[n];
        RandomAmbientSoundObject sound = currentSoundList[soundIndex];

        StartCoroutine(PlayAmbientSound(sound.audioClip));
    }

    //picks a random cooldown time, plays the clip, waits for it to finish, then waits for the cooldown
    IEnumerator PlayAmbientSound(AudioClip clip)
    {
        playing = true;

        float cooldown = Random.Range(minCooldown, maxCooldown);

        if (clip != null)
        {
            Debug.Log("Playing " + clip.name + " with " + cooldown + " second cooldown");

            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length);
        }

        yield return new WaitForSeconds(cooldown);
        playing = false;
    }

    private List<RandomAmbientSoundObject> GetMapSpecificSounds()
    {
        if (SceneManager.GetActiveScene().name == "Modern Map")
        {
            return modernMapAmbientSounds;
        }
        else if (SceneManager.GetActiveScene().name == "PleistoceneMap")
        {
            return pastMapAmbientSounds;
        }
        else
        {
            return modernMapAmbientSounds;
        }
    }

    private List<RandomAmbientSoundObject> GetAllPlayableSounds(List<List<RandomAmbientSoundObject>> sounds)
    {
        List<RandomAmbientSoundObject> allSounds = new List<RandomAmbientSoundObject>();

        foreach (List<RandomAmbientSoundObject> l in sounds)
        {
            allSounds.AddRange(l);
        }

        return allSounds;
    }

    private void MakeFrequencyAccurateList(List<RandomAmbientSoundObject> sounds)
    {
        frequencyList.Clear();

        for (int i = 0; i < sounds.Count; i++)
        {
            float f = Mathf.Floor(sounds[i].frequency * 100f);
            for (int j = 1; j <= (int)f ; j++)
            {
                frequencyList.Add(i);
            }

        }
    }

}
