using System;
using System.Collections.Generic;
using Audio;
using Match3;
using Misc;
using Narration;
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
            for (int i = 0; i < lvls.Count; i++)
            {
                if (lvls[i] == lvl+1)
                {
                    QuestNode target = QuestManager.questManager.GETNode(ids[i]);
                    if(target.isComplete)
                        return;
                    target.AddCount(0, 1);
                    if (lvl == 0)
                    {
                        WPUnlockSerializer.wpUnlockSerializer.Unlock("PLLake");
                        NarrationManager.narrationManager.SetPlayability("astronarrdemo", true);
                    }
                    if (lvl == 3)
                    {
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
