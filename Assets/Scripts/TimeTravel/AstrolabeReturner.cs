using LoadGUIFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AstrolabeReturner : MonoBehaviour
{
    // Start is called before the first frame update


    public Narration.Narration BP11;

    void Start()
    {

        Vector3 lastModernPos = PlayerPositionManager.playerPositionManager.getCurrentPlayerPosition(0);

        if (!float.IsNegativeInfinity(lastModernPos.x) & BP11.HasPlayed())
        {
            // do whatever you need with it — e.g. move an object there,
            // compare it to something, teleport the player, etc.
            AstrolabeQueueManager.queueManager.EnqueueDestination(lastModernPos, 1);
        }
        else
        {
            // no modern position saved yet (e.g. fresh save file) — handle that case
        }


    }
}