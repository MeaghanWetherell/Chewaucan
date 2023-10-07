using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace QuestSystem
{
    public class InitQuestGUI : MonoBehaviour
    {
        public Vector3 start;

        public GameObject questButton;

        public Transform scrollContent;

        [Tooltip("Height of a button")]
        public int height;
        
        void Start()
        {
            List<QuestNode> nodes = QuestManager.questManager.getQuests();
            int num = 0;
            foreach (QuestNode node in nodes)
            {
                createButtonPrefab(node, num);
                num++;
            }
        }

        private void createButtonPrefab(QuestNode node, int num)
        {
            GameObject newButton = Instantiate(questButton, scrollContent, false);
            //var position = start;
            //position.y += 125-height*num;
            //newButton.transform.position = position;
            newButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = node.name;
            newButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = node.shortDescription;
            newButton.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = node.count+"/"+node.requiredCount+" "+node.objective;
            if (node.isComplete)
            {
                newButton.transform.GetChild(3).gameObject.SetActive(true);
            }
            newButton.GetComponent<QuestButton>().node = node;
        }
    }
}
