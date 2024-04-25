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

    float _playSpeed = 0.9f;
    List<AudioClip> _clipListStep;
    List<AudioClip> _clipListJump;
    List<AudioClip> _clipListLand;
    AudioSource _playerAudio;

    CheckGroundTexture _groundTexture;

    private bool _isPlaying;

    public MovementSounds defaultSounds;

    public AudioClip swimSounds;

    private void Start()
    {
        _playerAudio = GetComponent<AudioSource>();
        _groundTexture = GetComponent<CheckGroundTexture>();
        _isPlaying = false;
    }

    //Plays the walking sounds if the coroutine is not already running
    public void PlayWalkingSound()
    {
        if (!_isPlaying)
        {
            float[] values = _groundTexture.GetValues();
            SetSoundList(values);
            StartCoroutine(PlaySound(_clipListStep));
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
            StartCoroutine(SwimSound());
        }
    }

    IEnumerator SwimSound()
    {
        _isPlaying = true;

        _playerAudio.clip = swimSounds;
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

        Debug.Log("     Playing " + clip.name);

        yield return new WaitForSeconds(_playerAudio.clip.length * _playSpeed);

        _isPlaying = false;
    }

    AudioClip GetClip(List<AudioClip> clipArray)
    {
        AudioClip selectedClip = clipArray[Random.Range(0, clipArray.Count)];
        return selectedClip;
    }

    public void SetPlaySpeed(float s)
    {
        _playSpeed = s;
    }

    /*
     * Returns the proper array of audio clips for walking, jumping, and landing
     * depending on the layer name of the texture on the terrain
     */
    void SetSoundList(float[] textureVals)
    {
        string layerName = _groundTexture.GetCurrentLayerName();
        Debug.Log(layerName);

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
                    return;
                }
            }
        }

        // default if no match is found
        _clipListStep = defaultSounds.stepSounds;
        _clipListJump = defaultSounds.jumpSounds;
        _clipListLand = defaultSounds.landSounds;

    }
}
