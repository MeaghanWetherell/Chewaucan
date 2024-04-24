using Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// script for ambient sound effects that play at random intervals while exploring the map
// this script is attached to an empty game object which is a child of the Player prefab

public class RandomAmbientSound : MonoBehaviour
{
    [Tooltip("Audio clips to play in both maps")]
    public List<RandomAmbientSoundObject> generalAmbientSounds = new List<RandomAmbientSoundObject>();

    [Tooltip("Audio clips to play only in the modern map")]
    public List<RandomAmbientSoundObject> modernMapAmbientSounds = new List<RandomAmbientSoundObject>();

    // not using the pleistocene map much yet, but might need it in the future
    [Tooltip("Audio clips to play only in the pleistocene map")]
    public List<RandomAmbientSoundObject> pleistoceneMapAmbientSounds = new List<RandomAmbientSoundObject>();

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
    private bool paused;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playing = false;
        paused = false;
        StartCoroutine(PlayAmbientSound(null));

        List<List<RandomAmbientSoundObject>> soundLists = new()
        {
            GetMapSpecificSounds(),
            generalAmbientSounds
        };

        GetAllPlayableSounds(soundLists); // set currentSoundList to contain all playable sounds in this scene

        PauseCallback.pauseManager.SubscribeToPause(OnPause);
        PauseCallback.pauseManager.SubscribeToResume(OnResume);
    }

    private void OnDestroy()
    {
        PauseCallback.pauseManager.UnsubToPause(OnPause);
        PauseCallback.pauseManager.UnsubToResume(OnResume);
    }

    private void Update()
    {
        if (!playing && !audioSource.isPlaying && !paused)
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

            // wait until the audio is no longer playing and the game is unpaused
            yield return new WaitUntil(() => (!audioSource.isPlaying && !paused));
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
            return pleistoceneMapAmbientSounds;
        }
        else
        {
            return modernMapAmbientSounds;
        }
    }

    private void GetAllPlayableSounds(List<List<RandomAmbientSoundObject>> sounds)
    {
        currentSoundList.Clear();

        foreach (List<RandomAmbientSoundObject> l in sounds)
        {
            currentSoundList.AddRange(l);
        }
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
