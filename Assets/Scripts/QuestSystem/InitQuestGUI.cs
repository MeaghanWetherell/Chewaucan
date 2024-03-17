using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace QuestSystem
{
    //initializes the buttons on the quest GUI
    public class InitQuestGUI : MonoBehaviour
    {
        //quest button prefab
        public GameObject questButton;

        //scroll under which to instantiate buttons
        public Transform scrollContent;

        //when the quest gui loads, initialize it
        void Start()
        {
            List<QuestNode> nodes = QuestManager.questManager.GETQuests();
            foreach (QuestNode node in nodes)
            {
                CreateButtonPrefab(node);
            }
        }

        //creates a button prefab with the details of the passed quest
        private void CreateButtonPrefab(QuestNode node)
        {
            GameObject newButton = Instantiate(questButton, scrollContent, false);
            newButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = node.MyObj.questName;
            newButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = node.shortDescription;
            TextMeshProUGUI text = newButton.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            text.text = node.counts[0] + "/" + node.MyObj.countsRequired[0] + " " + node.MyObj.objectives[0];
            for (int i = 1; i < node.MyObj.objectives.Count; i++)
            {
                text.text += ", "+node.counts[i] + "/" + node.MyObj.countsRequired[i] + " " + node.MyObj.objectives[i];
            }
            if (node.isComplete)
            {
                newButton.transform.GetChild(3).gameObject.SetActive(true);
            }
            newButton.GetComponent<QuestButton>().node = node;
        }
    }
}
