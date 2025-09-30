using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Audio;
using Match3;
using Misc;
using Narration;
using TimeTravel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace QuestSystem.Quests.QScripts
{
    public class MatchLvlComplete : MonoBehaviour
    {
        [Tooltip("Levels that trigger quest completion")]public List<int> lvls;

        [Tooltip("IDs of the quests to complete, matching the level that needs to be completed by index")] public List<String> ids;

        public QuestObj fishQuestObj;

        public Narration.Narration J6;

        public Narration.Narration J4;

        public Narration.Narration J3;

        public Narration.Narration J2;

        public string plateauQuestId;

        public void OnEnable()
        {
            MatchLevelManager.matchLevelManager.OnComplete.AddListener(OnLvlComplete);
        }

        public void OnDisable()
        {
            MatchLevelManager.matchLevelManager.OnComplete.RemoveListener(OnLvlComplete);
        }

        private void OnLvlComplete(int lvl)
        {
            if (lvl == 0)
            {
                if (J2.GetPlayability())
                {
                    J2.Begin();
                    J2.SetPlayability(false);
                }
            }
            for (int i = 0; i < lvls.Count; i++)
            {
                if (lvls[i] == lvl+1)
                {
                    QuestNode target = QuestManager.questManager.GETNode(ids[i]);
                    if(target.isComplete)
                        return;
                    target.AddCount(0, 1);
                    
                    if (lvl == 1)
                    {
                        if (J3.GetPlayability())
                        {
                            J3.Begin();
                            J3.SetPlayability(false);
                            J4.SetPlayability(true);
                            v3Wrapper toSerialize = new v3Wrapper(new Vector3(814,79,-340));
                            string json = JsonSerializer.Serialize(toSerialize);
                            string savePath = SaveHandler.saveHandler.getSavePath();
                            File.WriteAllText(savePath+"/astrolabeteleposition"+(2)+".json", json);
                            WPUnlockSerializer.wpUnlockSerializer.Unlock("MammothsWP");
                        }
                        QuestManager.questManager.GETNode("match31").UnlockUpdate(1);
                    }
                    if (lvl == 3)
                    {
                        J6.Begin();
                        QuestManager.questManager.CreateQuestNode(fishQuestObj);
                        GameObject popUp = GameObject.Find("PopUp(Clone)");
                        if (popUp != null)
                        {
                            popUp.GetComponentInChildren<Button>().onClick.AddListener(() => {
                                SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");});
                        }
                    }
                    if (ids[i] == fishQuestObj.uniqueID)
                    {
                        QuestNode plateauNode = QuestManager.questManager.GETNode(plateauQuestId);
                        if (plateauNode is {isComplete: true})
                        {
                            //TODO: narration trigger
                        }
                        else
                        {
                            //TODO: narration trigger    
                        }
                    }
                }
            }
        }
    }
}
