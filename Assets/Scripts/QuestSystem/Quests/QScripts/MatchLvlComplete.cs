using System;
using System.Collections.Generic;
using Match3;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem.Quests.QScripts
{
    public class MatchLvlComplete : MonoBehaviour
    {
        public static MatchLvlComplete matchLvlComplete;
        
        [Tooltip("Levels that trigger quest completion")]public List<int> lvls;

        [Tooltip("IDs of the quests to complete, matching the level that needs to be completed by index")] public List<String> ids;

        public QuestObj fishQuestObj;

        public string plateauQuestId;

        public void Awake()
        {
            matchLvlComplete = this;
        }

        public void OnLvlComplete()
        {
            int lvl = MatchLevelManager.matchLevelManager.curIndex;
            for (int i = 0; i < lvls.Count; i++)
            {
                if (lvls[i] == lvl+1)
                {
                    QuestNode target = QuestManager.questManager.GETNode(ids[i]);
                    if(target.isComplete)
                        return;
                    target.AddCount(0, 1);
                    QuestManager.questManager.CreateQuestNode(fishQuestObj);
                    GameObject popUp = GameObject.Find("PopUp(Clone)");
                    if (popUp != null)
                    {
                        popUp.GetComponentInChildren<Button>().onClick.AddListener(() => {
                            SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");});
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
