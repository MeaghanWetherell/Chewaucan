using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LoadGUIFolder;
using UnityEngine;
using UnityEngine.Events;

//prevent getting stuck in pleistocene if leaving before bp11 plays
public class BpileHelper2 : MonoBehaviour
{
    public Narration.Narration BP11;
    
    public Narration.Narration BP12;
    
    public Vector3 BPilePlayerPosition;
    void Start()
    {
        if(!BP11.HasPlayed())
            BP11.addToOnComplete(new List<UnityAction<string>>{
            str =>
            {
                //Debug.Log("Ran BP11 OnComp");
                LoadGUIManager.loadGUIManager.InstantiatePopUp("Back to the Present!", "Open your astrolabe and return to the present!");
                v3Wrapper toSerialize = new v3Wrapper(BPilePlayerPosition);
                string json = JsonSerializer.Serialize(toSerialize);
                string savePath = SaveHandler.saveHandler.getSavePath();
                File.WriteAllText(savePath+"/astrolabeteleposition1.json", json);
                BP12.SetPlayability(true);
            }});
    }

}
