using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSoundEffects : MonoBehaviour
{
    float _playSpeed = 0.9f;
    List<AudioClip> _clipListStep;
    List<AudioClip> _clipListJump;
    List<AudioClip> _clipListLand;
    AudioClip _previousClip;
    AudioSource _playerAudio;

    CheckGroundTexture _groundTexture;

    private bool _isPlaying;

    public MovementSounds rockSounds;
    public MovementSounds grassSounds;
    public MovementSounds leafSounds;
    public MovementSounds soilSounds;
    public MovementSounds gravelSounds;
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
            //Debug.Log(values[0]+" "+ values[1] + " " + values[2] + " " + values[3] + " " + values[4] + " " + values[5] + " " + values[6] + " " + values[7]);
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

        yield return new WaitForSeconds(_playerAudio.clip.length);

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
        yield return new WaitForSeconds(_playerAudio.clip.length * _playSpeed);

        _isPlaying = false;
    }

    AudioClip GetClip(List<AudioClip> clipArray)
    {
        int attempts = 3;
        AudioClip selectedClip = clipArray[Random.Range(0, clipArray.Count - 1)];
        while (selectedClip == _previousClip && attempts > 0)
        {
            selectedClip =
            clipArray[Random.Range(0, clipArray.Count - 1)];

            attempts--;
        }

        _previousClip = selectedClip;
        return selectedClip;
    }

    public void SetPlaySpeed(float s)
    {
        _playSpeed = s;
    }

    void SetSoundList(float[] textureVals)
    {
        if (textureVals[0] > 0.5f)
        {
            _clipListStep = gravelSounds.stepSounds;
            _clipListJump = gravelSounds.jumpSounds;
            _clipListLand = gravelSounds.landSounds;
            return;
        }

        if (textureVals[1] > 0.5f)
        {
            _clipListStep = soilSounds.stepSounds;
            _clipListJump = soilSounds.jumpSounds;
            _clipListLand = soilSounds.landSounds;
            return;
        }

        if (textureVals[7] > 0.5f)
        {
            _clipListStep = grassSounds.stepSounds;
            _clipListJump = grassSounds.jumpSounds;
            _clipListLand = grassSounds.landSounds;
            return;
        }

        //indexes 2-6 are rock textures, so it is treated as the default sounds to play
        //if none of the above if statements are true
        _clipListStep = rockSounds.stepSounds;
        _clipListJump = rockSounds.jumpSounds;
        _clipListLand = rockSounds.landSounds;
    }
}
