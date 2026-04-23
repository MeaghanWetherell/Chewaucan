using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LoadGUIFolder;
using TimeTravel;
using UnityEngine;
using UnityEngine.Events;

//prevents getting stuck in pleistocene if leaving before bp11 plays
public class BpileHelper2 : MonoBehaviour
{
    public Narration.Narration BP11;
    
    public Narration.Narration BP12;
    
    public Vector3 BPilePlayerPosition;
    void Start()
    {
        if (!BP11.HasPlayed())
        {
            if(BonepileScript.scriptSingleton == null)
                BP11.addToOnComplete(new List<UnityAction<string>>{
                    str =>
                    {
                        LoadGUIManager.loadGUIManager.InstantiatePopUp("Back to the Present!", "Open your astrolabe and return to the present!");
                        AstrolabeQueueManager.queueManager.EnqueueDestination(BPilePlayerPosition, 1);
                        MapUIController.canOpenMap = true;
                        BP12.SetPlayability(true);
                    }, str => { WPUnlockSerializer.wpUnlockSerializer.unlockAllModern = true;}});
        }
        
    }

}
