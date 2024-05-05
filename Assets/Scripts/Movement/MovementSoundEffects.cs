using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementSoundEffects : MonoBehaviour
{
    [System.Serializable]
    public class MovementSoundInfo
    {
        public MovementSounds sounds;

        [Tooltip("keywords in the names of the related layers to identify these sounds")]
        [SerializeField] List<string> keywords = new List<string>();

        public List<string> getKeywords() { return keywords; }
    }

    [SerializeField] List<MovementSoundInfo> movementSoundInfo = new();

    List<AudioClip> _clipListStep;
    List<AudioClip> _clipListJump;
    List<AudioClip> _clipListLand;
    List<AudioClip> _clipListSprint;
    AudioSource _playerAudio;

    CheckGroundTexture _groundTexture;

    private bool _isPlaying;
    private bool isSprinting;

    public MovementSounds defaultSounds;

    public List<AudioClip> swimSounds;
    public List<AudioClip> swimSprintSounds;

    private void Start()
    {
        _playerAudio = GetComponent<AudioSource>();
        _groundTexture = GetComponent<CheckGroundTexture>();
        _isPlaying = false;
        isSprinting = false;
    }

    //Plays the walking sounds if the coroutine is not already running
    public void PlayWalkingSound()
    {
        if (!_isPlaying)
        {
            float[] values = _groundTexture.GetValues();
            SetSoundList(values);
            if (!isSprinting)
            {
                StartCoroutine(PlaySound(_clipListStep));
            }
            else
            {
                StartCoroutine(PlaySound(_clipListSprint));
            }
        }
    }

    public void PlayJumpSound()
    {
        StopAllCoroutines();
        _isPlaying = false;
        float[] values = _groundTexture.GetValues();
        SetSoundList(values);
        StartCoroutine(PlaySound(_clipListJump));
        
    }

    public void PlayLandSound()
    {
        if (!_isPlaying)
        {
            float[] values = _groundTexture.GetValues();
            SetSoundList(values);
            StartCoroutine(PlaySound(_clipListLand));
        }
    }

    public void PlaySwimSound()
    {
        if (!_isPlaying)
        {
            if (!isSprinting)
            {
                StartCoroutine(SwimSound(swimSounds));
            }
            else
            {
                StartCoroutine(SwimSound(swimSprintSounds));
            }
        }
    }

    IEnumerator SwimSound(List<AudioClip> sounds)
    {
        _isPlaying = true;

        _playerAudio.clip = sounds[Random.Range(0, sounds.Count)];
        _playerAudio.Play();

        yield return new WaitUntil(() => (!_playerAudio.isPlaying));

        _isPlaying = false;
    }

    /**
     * Plays an audio clip and waits for it to finish playing.
     */
    IEnumerator PlaySound(List<AudioClip> clipList)
    {
        _isPlaying = true;

        AudioClip clip = GetClip(clipList);

        _playerAudio.clip = clip;
        _playerAudio.Play();

        //Debug.Log("     Playing " + clip.name);

        yield return new WaitForSeconds(_playerAudio.clip.length);

        _isPlaying = false;
    }

    AudioClip GetClip(List<AudioClip> clipArray)
    {
        AudioClip selectedClip = clipArray[Random.Range(0, clipArray.Count)];
        return selectedClip;
    }

    public void SetIsSprinting(bool sprint)
    {
        isSprinting = sprint;
    }

    /*
     * Returns the proper array of audio clips for walking, jumping, and landing
     * depending on the layer name of the texture on the terrain
     */
    void SetSoundList(float[] textureVals)
    {
        string layerName = _groundTexture.GetCurrentLayerName();
        //Debug.Log(layerName);

        foreach (MovementSoundInfo info in movementSoundInfo)
        {
            foreach(string keyword in info.getKeywords())
            {
                layerName = layerName.ToLower();
                if (layerName.Contains(keyword.ToLower()))
                {
                    _clipListStep = info.sounds.stepSounds;
                    _clipListJump = info.sounds.jumpSounds;
                    _clipListLand = info.sounds.landSounds;
                    _clipListSprint = info.sounds.sprintSounds;
                    return;
                }
            }
        }

        // default if no match is found
        _clipListStep = defaultSounds.stepSounds;
        _clipListJump = defaultSounds.jumpSounds;
        _clipListLand = defaultSounds.landSounds;
        _clipListSprint = defaultSounds.sprintSounds;

    }
}
