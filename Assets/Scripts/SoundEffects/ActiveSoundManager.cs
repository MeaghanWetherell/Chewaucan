using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Used to enable and disable ambient sounds based on player activites
 * mostly if the player goes underwater, but may also happen in caves.
 */
public class ActiveSoundManager : MonoBehaviour
{
    // game object handing random ambient sounds
    [SerializeField] RandomAmbientSound ambientSoundManager;


    /*
     * Disables random ambient sounds when called. For example, when the player
     * is underwater (we don't want bird noises playing when underwater)
     * This should be called from other scripts when entering a place where you don't
     * want these sounds to play
     */
    public void DisableAmbientSounds()
    {
        if (ambientSoundManager != null)
        {
            ambientSoundManager.gameObject.SetActive(false);
        }
    }

    /*
     * Enables the random ambient sounds.
     * This should be called from other scripts where you want ambient sounds to play
     */
    public void EnableAmbientSounds()
    {
        if (ambientSoundManager != null)
        {
            ambientSoundManager.gameObject.SetActive(true);
        }
    }
}
