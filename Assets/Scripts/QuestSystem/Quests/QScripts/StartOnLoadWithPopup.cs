using System;
using System.Collections.Generic;
using Misc;
using NUnit.Framework;
using UnityEngine.Events;

namespace QuestSystem.Quests.QScripts
{
    public class StartOnLoadWithPopup : QuestHandler
    {
        private static Dictionary<string, string[]> popups = new Dictionary<string, string[]>();

        public string initTitle;

        public string initMsg;

        public string compTitle;

        public string compMsg;

        private void Start()
        {
            if ((!initTitle.Equals("") || !initMsg.Equals("")) && !popups.ContainsKey(questData.receivedNarration.narrationClip.name))
            {
                List<UnityAction<string>> temp = new List<UnityAction<string>>();
                temp.Add(PopUp);
                questData.receivedNarration.addToOnComplete(temp);
                popups.Add(questData.receivedNarration.narrationClip.name, new string[2] {initTitle, initMsg});
            }
            if ((!compTitle.Equals("") || !compMsg.Equals("")) && !popups.ContainsKey(questData.completeNarration.narrationClip.name))
            {
                List<UnityAction<string>> temp = new List<UnityAction<string>>();
                temp.Add(PopUp);
                questData.completeNarration.addToOnComplete(temp);
                popups.Add(questData.completeNarration.narrationClip.name, new string[2] {initTitle, initMsg});
            }
            StartQuest();
        }

        private static void PopUp(string title)
        {
            LoadGUIManager.loadGUIManager.InstantiatePopUp(popups[title][0], popups[title][1]);
        }
    }
}
