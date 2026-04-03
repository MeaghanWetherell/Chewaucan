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
        [Tooltip("Levels that trigger quest completion")]public List<int> questCompLvls;

        [Tooltip("IDs of the quests to complete, matching the level that needs to be completed by index")] public List<String> questCompIds;
        
        [Tooltip("Levels that trigger the player gaining a quest")]public List<int> questUnlockLvls;

        [Tooltip("Quest objs of the quests to unlock, matching the level that needs to be completed by index")] public List<QuestObj> questUnlockObs;

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
            for (int i = 0; i < questUnlockLvls.Count; i++)
            {
                if (questUnlockLvls[i] == lvl + 1)
                {
                    QuestManager.questManager.CreateQuestNode(questUnlockObs[i]);
                }
            }
            for (int i = 0; i < questCompLvls.Count; i++)
            {
                if (questCompLvls[i] == lvl+1)
                {
                    QuestNode target = QuestManager.questManager.GETNode(questCompIds[i]);
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
                            WPUnlockSerializer.wpUnlockSerializer.Unlock("MammothsWP");
                            LoadGUIManager.loadGUIManager.SubToTopPopUp(str => {SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map"); });
                            QuestManager.questManager.GETNode("MainQuest").UnlockUpdate(1);
                        }
                    }
                }
            }
        }
    }
}
