using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using TMPro;
using UnityEngine;

namespace Misc
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager hudManager;
        
        public List<GameObject> pins;

        public TextMeshProUGUI messageText;

        private void Awake()
        {
            hudManager = this;
        }

        private void OnEnable()
        {
            messageText.gameObject.transform.parent.gameObject.SetActive(false);
            StartCoroutine(waitForQuestLoad());
        }

        public void sendMessage(String message)
        {
            messageText.gameObject.transform.parent.gameObject.SetActive(true);
            messageText.text = message;
        }

        public void closeMessage()
        {
            messageText.gameObject.transform.parent.gameObject.SetActive(false);
        }

        //if the quest manager hasn't been initialized yet, wait a frame to set up quest pins
        private IEnumerator waitForQuestLoad()
        {
            while (QuestManager.questManager == null)
            {
                yield return new WaitForSeconds(0);
            }
            resetPins();
        }

        public void resetPins()
        {
            QuestNode[] pinNodes = QuestManager.questManager.getPinNodes();
            pinNodes.insertionSort();
            GameObject pinField = transform.GetChild(0).gameObject;
            if (pinNodes[0] == null)
            {
                pinField.SetActive(false);
                return;
            }
            pinField.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                QuestNode cur = pinNodes[i];
                Transform pin = pins[i].transform;
                if (cur == null)
                {
                    pin.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
                    pin.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
                    pin.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "";
                }
                else
                {
                    pin.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = cur.name;
                    //removed so objectives don't get compressed
                    //pin.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = cur.shortDescription;
                    TextMeshProUGUI text = pin.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
                    text.text = "";
                    for (int j = 0; j < cur.objectives.Count; j++)
                    {
                        text.text += +cur.counts[j] + "/" + cur.requiredCounts[j] + " " + cur.objectives[j]+"\n";
                    }
                }
            }
        }
    }
}
