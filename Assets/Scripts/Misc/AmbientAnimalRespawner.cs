using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAnimalRespawner : MonoBehaviour
{
    public static AmbientAnimalRespawner respawner;

    private void Awake()
    {
        respawner = this;
    }

    public IEnumerator Respawn(AnimalRun respawn, float TimeToRespawn)
    {
        yield return new WaitForSeconds(TimeToRespawn);
        respawn.Respawn();
    }
}
