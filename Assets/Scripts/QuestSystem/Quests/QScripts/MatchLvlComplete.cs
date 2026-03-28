using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Audio;
using LoadGUIFolder;
using Match3;
using Misc;
using Narration;
using TimeTravel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace QuestSystem.Quests.QScripts
{
    //interface between match3 and the quest system
    public class MatchLvlComplete : MonoBehaviour
    {
        [Tooltip("Levels that trigger quest completion (do not use 0 based indexing)")]public List<int> qCompletelvls;

        [Tooltip("IDs of the quests to complete, matching the level that needs to be completed by index")] public List<String> compIds;
        
        [Tooltip("Levels that give the player a quest (do not use 0 based indexing)")]public List<int> qGainlvls;

        [Tooltip("IDs of the quests to give, matching the level that needs to be completed by index")] public List<QuestObj> gainObjs;

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

        //triggers quest updates and progress per level
        private void OnLvlComplete(int lvl)
        {
            if (lvl == 0)
            {
                /* deprecated
                if (J2.GetPlayability())
                {
                    J2.Begin();
                    J2.SetPlayability(false);
                }
                QuestManager.questManager.GETNode("match31").UnlockUpdate(1);
                 */
            }
            for(int i = 0; i < qGainlvls.Count; i++)
            {
                if (qGainlvls[i] == lvl + 1)
                {
                    QuestManager.questManager.CreateQuestNode(gainObjs[i]);
                }
            }
            for (int i = 0; i < qCompletelvls.Count; i++)
            {
                
                if (qCompletelvls[i] == lvl+1)
                {
                    QuestNode target = QuestManager.questManager.GETNode(compIds[i]);
                    if(target.isComplete)
                        return;
                    target.AddCount(0, 1);
                    
                    if (lvl == 0)
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
                            LoadGUIManager.loadGUIManager.SubToTopPopUp(str => {SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map"); });
                            QuestManager.questManager.GETNode("MainQuest").UnlockUpdate(1);
                        }
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
                }
            }
        }
    }
}
