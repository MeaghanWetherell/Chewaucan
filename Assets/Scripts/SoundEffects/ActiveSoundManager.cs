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

    [SerializeField] RandomAmbientSound ambientSoundManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableAmbientSounds()
    {
        if (ambientSoundManager != null)
        {
            ambientSoundManager.gameObject.SetActive(false);
        }
    }

    public void EnableAmbientSounds()
    {
        if (ambientSoundManager != null)
        {
            ambientSoundManager.gameObject.SetActive(true);
        }
    }
}
