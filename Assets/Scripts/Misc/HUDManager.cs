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

        public GameObject subtitleBG;

        public GameObject skipBG;

        private void Awake()
        {
            hudManager = this;
            StartCoroutine(WaitForQuestLoad());
        }

        private void Start()
        {
            PauseCallback.pauseManager.SubscribeToPause(DeActivate);
            PauseCallback.pauseManager.SubscribeToResume(Activate);
        }

        private void OnDestroy()
        {
            PauseCallback.pauseManager.UnsubToPause(DeActivate);
            PauseCallback.pauseManager.UnsubToResume(Activate);
        }

        private void OnEnable()
        {
            messageText.gameObject.transform.parent.gameObject.SetActive(false);
        }

        public void DisplayMessageToHUD(String message)
        {
            messageText.gameObject.transform.parent.gameObject.SetActive(true);
            messageText.text = message;
        }

        public void CloseMessage()
        {
            messageText.gameObject.transform.parent.gameObject.SetActive(false);
        }

        //if the quest manager hasn't been initialized yet, wait a frame to set up quest pins
        private IEnumerator WaitForQuestLoad()
        {
            while (QuestManager.questManager == null)
            {
                yield return new WaitForSeconds(0);
            }
            ResetPins();
        }

        public void ResetPins()
        {
            QuestNode[] pinNodes = QuestManager.questManager.GETPinNodes();
            pinNodes.InsertionSort();
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

        private void Activate()
        {
            gameObject.GetComponent<Canvas>().enabled = true;
            StartCoroutine(WaitForQuestLoad());
        }

        private void DeActivate()
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }
    }
}
